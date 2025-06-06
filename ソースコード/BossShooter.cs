using UnityEngine;
using System.Collections;

public class BossShooter : MonoBehaviour
{
    public enum BossFireMode { HighCard, Pair, Straight, Triple }
    public BossFireMode fireMode = BossFireMode.HighCard;
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    public GameObject bulletPrefab3;
    public Transform firePoint;
    public float fireInterval = 2f;
    private float fireTimer = 0f;
    public float lightTime = 0f;

    public float range = 5f;
    public int count = 10;

    private BossCardManager bcm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (firePoint == null)
        {
            var point = GameObject.Find("BossFirePoint");
            if (point != null)
            {
                firePoint = point.transform;
            }
        }

        bcm = GetComponent<BossCardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 手札のタイプに応じて攻撃モードを切り替える
        if (bcm.handCards.Count == 3)
        {
            HandType type = CardEvaluator.Evaluate(bcm.handCards);
            var shooter = GetComponent<BossShooter>();
            if (shooter != null)
            {
                switch (type)
                {
                    case HandType.Triple:
                        shooter.fireMode = BossShooter.BossFireMode.Triple;
                        break;
                    case HandType.Straight:
                        shooter.fireMode = BossShooter.BossFireMode.Straight;
                        break;
                    case HandType.Pair:
                        shooter.fireMode = BossShooter.BossFireMode.Pair;
                        break;
                    default:
                        shooter.fireMode = BossShooter.BossFireMode.HighCard;
                        break;
                }
            }
        }

        fireTimer += Time.deltaTime;
        lightTime += Time.deltaTime;
        if (fireTimer >= fireInterval && bulletPrefab != null && firePoint != null)
        {
            fireTimer = 0f;
            if (fireMode == BossFireMode.HighCard)
            {
                FireBullet(Vector2.down, bulletPrefab);
            }
            else if (fireMode == BossFireMode.Pair)
            {
                Vector2[] directions = { Vector2.down, new Vector2(-0.7f, -1f), new Vector2(0.7f, -1f) };
                foreach (var dir in directions)
                {
                    FireBullet(dir, bulletPrefab);
                }
            }
            else if (fireMode == BossFireMode.Straight)
            {
                Vector2[] directions = {
                    Vector2.up, Vector2.down, Vector2.left, Vector2.right,
                    new Vector2(1,1), new Vector2(-1,1), new Vector2(1,-1), new Vector2(-1,-1)
                };
                foreach (var dir in directions)
                {
                    FireBullet2(dir, bulletPrefab2);
                }
            }
            else if (fireMode == BossFireMode.Triple && lightTime >= fireInterval * 2)
            {
                StartCoroutine(FireThunder());
                lightTime = 0f;
            }
        }
    }

    void FireBullet(Vector2 direction, GameObject bulletPrefab)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Init(direction, GameManager.Instance.MapBounds, BulletOwner.Boss);
        }
    }

    void FireBullet2(Vector2 direction, GameObject bulletPrefab)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<Bullet2>();
        if (bulletScript != null)
        {
            bulletScript.Init(direction, GameManager.Instance.MapBounds, BulletOwner.Boss);
        }
    }

    IEnumerator FireThunder()
    {
        // 自分の位置に基づいて,その周囲に雷の弾幕を落とす
        Vector3 center = transform.position;

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-range, range), Random.Range(-range, range), 0f);
            Vector3 spawnPos = center + offset;
            GameObject bullet = Instantiate(bulletPrefab3, spawnPos, Quaternion.identity);
            var bulletScript = bullet.GetComponent<Bullet3>();
            if (bulletScript != null)
            {
                bulletScript.Init(Vector2.down, GameManager.Instance.MapBounds, BulletOwner.Boss);
            }
        }
        yield break;
    }
}
