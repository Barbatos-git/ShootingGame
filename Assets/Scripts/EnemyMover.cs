using UnityEngine;
using System.Collections;

public class EnemyMover : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 moveDir = Vector2.down;

    public int hp = 10;
    public int atk = 10;
    public GameObject explosionPrefab;

    public float maxLifeTime = 10f;
    private float lifeTimer = 0f;
    private bool isDead = false;  // Die()の重複呼び出しを防止する

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);

        lifeTimer += Time.deltaTime;

        // 破棄の保険
        if (lifeTimer >= maxLifeTime)
        {
            Die();
        }
    }

    public void SetDirection(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    // 当たり判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            var bullet = other.GetComponent<Bullet>();
            TakeDamage(bullet.atk);
            Destroy(other.gameObject); // 弹を破棄する
        }
        else if (other.CompareTag("Player"))
        {
            Die(killedByPlayer: true); // ぶつかって即死
            var player = other.GetComponent<PlayerDamageHandler>();
            if (player != null)
            {
                player.ReceiveDamage(atk);
            }
        }
        else if (other.CompareTag("Bullet2"))
        {
            var bullet = other.GetComponent<Bullet2>();
            TakeDamage(bullet.atk);
            Destroy(other.gameObject); // 弹を破棄する
        }
        else if (other.CompareTag("Bullet3"))
        {
            var bullet = other.GetComponent<Bullet3>();
            TakeDamage(bullet.atk);
            Destroy(other.gameObject); // 弹を破棄する
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die(killedByPlayer: true);
        }
    }

    void Die(bool killedByPlayer = false)
    {
        if (isDead) return;
        isDead = true;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        if (killedByPlayer)
        {
            EnemyKillCounter.Instance?.RegisterKill();  // プレイヤーは撃殺する時に

            GameStatsUI statsUI = FindObjectOfType<GameStatsUI>();
            if (statsUI != null)
            {
                statsUI.AddKill();
                statsUI.AddScore(10);
            }
        }

        Destroy(gameObject);
    }
}
