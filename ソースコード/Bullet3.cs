using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    public int atk = 40;
    private Bounds mapBounds;
    public BulletOwner owner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // マップ境界を超える→破棄
        if (!mapBounds.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }

    public void Init(Vector2 dir, Bounds bounds, BulletOwner bulletOwner)
    {
        mapBounds = bounds;
        owner = bulletOwner;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (owner == BulletOwner.Boss && other.CompareTag("Player"))
        {
            var dmg = other.GetComponent<PlayerDamageHandler>();
            if (dmg != null) dmg.ReceiveDamage(atk);
            Destroy(gameObject);
        }
        else if (owner == BulletOwner.Player && other.CompareTag("Boss"))
        {
            var boss = other.GetComponent<BossHealth>();
            if (boss != null) boss.TakeDamage(atk);
            Destroy(gameObject);
        }
    }

    public void AutoDestroy()
    {
        Destroy(gameObject);
    }
}
