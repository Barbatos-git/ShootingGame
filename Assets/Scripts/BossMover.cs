using UnityEngine;
using System.Collections;

public class BossMover : MonoBehaviour
{
    public float moveInterval = 2f;      // 移动頻度
    public float moveSpeed = 3f;         // 移动速度
    public float arrivalThreshold = 0.1f;

    private Vector2 targetPosition;
    private Bounds mapBounds;

    private bool isAlive = true;
    private Coroutine moveCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 地図の境界を取得する
        mapBounds = GameManager.Instance.MapBounds;
        moveCoroutine = StartCoroutine(MoveLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveLoop()
    {
        while (isAlive)
        {
            SetNewTarget();

            // 移動が完了するまで待つ
            while (Vector2.Distance(transform.position, targetPosition) > arrivalThreshold)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(moveInterval);
        }
    }

    void SetNewTarget()
    {
        float margin = 1f;
        float minX = mapBounds.min.x + margin;
        float maxX = mapBounds.max.x - margin;
        float minY = (mapBounds.center.y + mapBounds.max.y) / 2f; // 上半部分
        float maxY = mapBounds.max.y - margin;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        targetPosition = new Vector2(x, y);
    }
    public void StopMoving()
    {
        isAlive = false;
        StopCoroutine(moveCoroutine);
    }
}
