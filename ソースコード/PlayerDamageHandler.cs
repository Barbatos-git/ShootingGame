using UnityEngine;
using System.Collections;

public class PlayerDamageHandler : MonoBehaviour
{
    public float invincibleTime = 0.5f;
    public AudioClip hitSFX;

    private bool isInvincible = false;
    private SpriteRenderer sr;
    private Color originalColor;
    private AudioSource audioSource;
    private HealthComponent health;
    public float flashInterval = 0.1f; // 点滅頻度

    [Header("爆発エフェクト")]
    public GameObject deathEffectPrefab;

    [Header("死亡音声")]
    public AudioClip deathSound;

    [Header("遅延破壊時間（秒）")]
    public float destroyDelay = 0.2f;

    [Header("バインディング血条 UI")]
    public PlayerHealthUI playerHealthUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        originalColor = sr.color;
        health = GetComponent<HealthComponent>();

        // バインディング血条 UI
        HealthComponent hc = GetComponent<HealthComponent>();
        if (hc != null && playerHealthUI != null)
        {
            playerHealthUI.BindTo(hc);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReceiveDamage(int amount)
    {
        if (isInvincible || health.GetCurrentHP() <= 0) return;

        health.TakeDamage(amount);

        if (hitSFX && audioSource)
            audioSource.PlayOneShot(hitSFX);

        StartCoroutine(InvincibilityFlash());
    }

    IEnumerator InvincibilityFlash()
    {
        isInvincible = true;
        float elapsed = 0f;
        bool white = true;

        while (elapsed < invincibleTime)
        {
            sr.color = white ? Color.red : originalColor;
            white = !white;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        sr.color = originalColor;
        isInvincible = false;
    }

    public void HandleDeath()
    {
        // 爆発エフェクト出す
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        // 音声プレイ
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // 移动が止まる
        PlayerController move = GetComponent<PlayerController>();
        if (move != null)
            move.DisableMovement();

        // プレイヤーを破棄する
        Destroy(gameObject);
    }
}
