using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public Vector2Int straightSpawnCountRange = new Vector2Int(1, 3);
    public Vector2Int diagonalSpawnCountRange = new Vector2Int(1, 2);
    public Vector2Int formationCountRange = new Vector2Int(3, 6);
    private Bounds mapBounds;
    private float enemyDiameter = 6f; // fallback デフォルト値
    private bool spawnEnabled = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        mapBounds = GameManager.Instance.MapBounds;

        // 敵の衝突半径を自動的に取得する
        CircleCollider2D col = enemyPrefab.GetComponent<CircleCollider2D>();
        if (col != null)
        {
            enemyDiameter = col.radius * 2f * enemyPrefab.transform.localScale.x; // スケーリングをかける
        }
        else
        {
            Debug.LogWarning("enemyPrefabにCircleCollider2Dはない。デフォルトピッチ6を使用する。");
        }

        StartCoroutine(SpawnLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (spawnEnabled)
            {
                SpawnStraightDown();
                yield return new WaitForSeconds(spawnInterval);

                SpawnDiagonal();
                yield return new WaitForSeconds(spawnInterval);

                SpawnFormation();
                yield return new WaitForSeconds(spawnInterval * 2);
            }
            else
            {
                yield return null;
            }
        }
    }

    // ① 直線に滑り込む
    void SpawnStraightDown()
    {
        int count = Random.Range(straightSpawnCountRange.x, straightSpawnCountRange.y + 1);
        float spacing = enemyDiameter * 1.1f;

        // Xの位置を固定する
        float x = Random.Range(mapBounds.min.x + enemyDiameter / 2f, mapBounds.max.x - enemyDiameter / 2f);

        // ホームY位置は画面外上方
        float startY = mapBounds.max.y + (enemyDiameter / 2f);

        for (int i = 0; i < count; i++)
        {
            float y = startY + i * spacing; // 垂直に並んで上に伸びている
            Vector2 spawnPos = new Vector2(x, y);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyMover>().SetDirection(Vector2.down);
        }
    }

    // ② 斜めに切り込む
    void SpawnDiagonal()
    {
        int count = Random.Range(diagonalSpawnCountRange.x, diagonalSpawnCountRange.y + 1);
        List<float> usedY = new List<float>();

        for (int i = 0; i < count; i++)
        {
            bool fromLeft = Random.value < 0.5f;
            float yMin = mapBounds.center.y;
            float yMax = mapBounds.max.y - enemyDiameter / 2f;

            int tryCount = 0;
            float y;
            do
            {
                y = Random.Range(yMin, yMax);
                tryCount++;
            } while (usedY.Exists(pos => Mathf.Abs(pos - y) < enemyDiameter) && tryCount < 10);

            usedY.Add(y);

            float x = fromLeft ? mapBounds.min.x - enemyDiameter / 2f : mapBounds.max.x + enemyDiameter / 2f;
            Vector2 spawnPos = new Vector2(x, y);
            Vector2 dir = fromLeft ? new Vector2(1, -1) : new Vector2(-1, -1);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyMover>().SetDirection(dir);
        }
    }

    // ③ 編隊生成
    void SpawnFormation()
    {
        int count = Random.Range(formationCountRange.x, formationCountRange.y + 1);
        float spacing = Mathf.Max(enemyDiameter, enemyDiameter * 1.1f);
        float totalWidth = (count - 1) * spacing;

        float startX = Random.Range(mapBounds.min.x + enemyDiameter / 2f, mapBounds.max.x - enemyDiameter / 2f - totalWidth);
        float y = mapBounds.max.y + enemyDiameter / 2f;

        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = new Vector2(startX + i * spacing, y);
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyMover>().SetDirection(Vector2.down);
        }
    }

    public void StopSpawning()
    {
        spawnEnabled = false;
    }
}
