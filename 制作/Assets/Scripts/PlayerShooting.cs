using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour
{
    public enum PlayerFireMode { HighCard, Pair, Straight, Triple }
    public PlayerFireMode fireMode = PlayerFireMode.HighCard;
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    public GameObject bulletPrefab3;
    public Transform firePoint;  // 弹発射位置
    public float fireCooldown = 0.2f;
    private float fireTimer = 0f;
    public float lightTime = 0f;

    public float range = 5f;
    public int count = 10;

    private PlayerCardManager pcm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pcm = FindObjectOfType<PlayerCardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        lightTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireCooldown)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        var handList = new List<CardData>();
        foreach (var card in pcm.GetHandCards()) 
        {
            if (card != null) handList.Add(card);
        }
        if (handList.Count < 3) return;
        HandType type = CardEvaluator.Evaluate(handList);
        // 手札のタイプに応じて攻撃モードを切り替える
        switch (type)
        {
            case HandType.Triple:
                fireMode = PlayerFireMode.Triple;
                break;
            case HandType.Straight:
                fireMode = PlayerFireMode.Straight;
                break;
            case HandType.Pair:
                fireMode = PlayerFireMode.Pair;
                break;
            default:
                fireMode = PlayerFireMode.HighCard;
                break;  
        }

        if (fireMode == PlayerFireMode.HighCard)
        {
            FireBullet(Vector2.up, bulletPrefab);
        }
        else if (fireMode == PlayerFireMode.Pair)
        {
            Vector2[] directions = { Vector2.up, new Vector2(-0.7f, 1f), new Vector2(0.7f, 1f) };
            foreach (var dir in directions)
            {
                FireBullet(dir, bulletPrefab);
            }
        }
        else if (fireMode == PlayerFireMode.Straight)
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
        else if (fireMode == PlayerFireMode.Triple && lightTime >= fireCooldown*3)
        {
            StartCoroutine(FireThunder());
            lightTime = 0f;
        }
    }

    void FireBullet(Vector2 direction, GameObject bulletPrefab)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Init(direction, GameManager.Instance.MapBounds, BulletOwner.Player);
        }
    }

    void FireBullet2(Vector2 direction, GameObject bulletPrefab)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<Bullet2>();
        if (bulletScript != null)
        {
            bulletScript.Init(direction, GameManager.Instance.MapBounds, BulletOwner.Player);
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
                bulletScript.Init(Vector2.down, GameManager.Instance.MapBounds, BulletOwner.Player);
            }
        }
        yield break;
    }
}
