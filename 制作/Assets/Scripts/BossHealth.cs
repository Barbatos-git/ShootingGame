using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.Audio;

public class BossHealth : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;
    public int atk = 40;
    public AudioClip hitSFX;
    private AudioSource audioSource;

    public GameObject deathEffect;
    public event Action<int, int> OnHealthChanged;

    [Header("死亡音声")]
    public AudioClip deathSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP, maxHP); // 始め更新
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        OnHealthChanged?.Invoke(currentHP, maxHP);
        if (hitSFX && audioSource)
            audioSource.PlayOneShot(hitSFX);

        if (currentHP <= 0)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.GameOverIsBoss();
            Die();
        }
    }

    void Die()
    {
        // 音声プレイ
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // 移动が止まる
        var boss = GetComponent<BossMover>();
        if (boss != null)
        {
            boss.StopMoving();
        }

        // 死亡エフェクトを出す
        if (deathEffect)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // 画面が振動する
        CameraShake.Instance?.Shake(1f, 0.2f);

        GameStatsUI statsUI = FindObjectOfType<GameStatsUI>();
        if (statsUI != null)
        {
            statsUI.AddKill();
            statsUI.AddScore(maxHP);
        }

        Destroy(gameObject); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var dmg = other.GetComponent<PlayerDamageHandler>();
            if (dmg != null) dmg.ReceiveDamage(atk);
        }
    }

    public int GetCurrentHP() => currentHP;
    public int GetMaxHP() => maxHP;
}
