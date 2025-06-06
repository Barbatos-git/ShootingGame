using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossSummoner : MonoBehaviour
{
    [Header("生成点")]
    public Transform spawnPoint;

    [Header("エフェクト")]
    public GameObject effectPrefab;

    [Header("Boss")]
    public GameObject bossPrefab;
    public float bossFadeInDuration = 2f;
    private GameObject currentEffect;

    public EnemySpawnManager spawnManager;

    public BossHealthUI bossHealthUI;

    public Transform bossCardArea;
    public Transform bossReserveArea;
    public CardFactory cardFactory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBossSequence()
    {
        currentEffect = Instantiate(effectPrefab, spawnPoint.position, Quaternion.identity);

        // ポイント:エフェクトをこのシナリオに戻します
        EffectController effectScript = currentEffect.GetComponent<EffectController>();
        if (effectScript != null)
        {
            effectScript.summoner = this;
        }
    }

    // Animation Event コールバック関数(エフェクトのアニメーションから呼び出します)
    public void OnEffectAnimationEnd()
    {
        if (currentEffect != null)
            Destroy(currentEffect);

        // 音楽を切り替える
        AudioManager.Instance?.PlayBossBGM();

        // 敵の生成停止をお知らせる
        spawnManager?.StopSpawning();


        // Bossが現れる
        StartCoroutine(FadeInBoss());
    }

    IEnumerator FadeInBoss()
    {
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        bossHealthUI.GetComponent<BossHealthUI>().ShowBossHP();

        // バインディング血条UI
        BossHealth bh = boss.GetComponent<BossHealth>();
        if (bh != null && bossHealthUI != null)
        {
            bossHealthUI.BindTo(bh);
        }

        SpriteRenderer sr = boss.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;

            float timer = 0f;
            while (timer < bossFadeInDuration)
            {
                float t = timer / bossFadeInDuration;
                c.a = Mathf.Lerp(0f, 1f, t);
                sr.color = c;
                timer += Time.deltaTime;
                yield return null;
            }

            c.a = 1f;
            sr.color = c;
        }
    }
}
