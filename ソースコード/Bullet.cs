using UnityEngine;

public enum BulletOwner { Player, Boss }

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public Vector2 moveDir = Vector2.up;
    private Bounds mapBounds;
    public int atk = 10;
    public BulletOwner owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 弹移动
        transform.Translate(moveDir * speed * Time.deltaTime);

        // マップ境界を超える→破棄
        if (!mapBounds.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    public void Init(Vector2 dir, Bounds bounds, BulletOwner bulletOwner)
    {
        moveDir = dir.normalized;
        mapBounds = bounds;
        owner = bulletOwner;

        Destroy(gameObject, lifeTime); // 破棄の保険
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
}
