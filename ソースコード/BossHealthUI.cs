using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Image targetImage; // 表示用のImageコンポーネント
    public Sprite[] healthSprites; // 0 = 空，11 = いっぱい
    public GameObject bossHPBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 最初は血条を隠する
        if (bossHPBar != null)
            bossHPBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BindTo(BossHealth boss)
    {
        boss.OnHealthChanged += UpdateHP;
        UpdateHP(boss.GetCurrentHP(), boss.GetMaxHP());
    }

    public void UpdateHP(int current, int max)
    {
        float percent = (float)current / max;
        int index = Mathf.Clamp(Mathf.RoundToInt(percent * (healthSprites.Length - 1)), 0, healthSprites.Length - 1);
        targetImage.sprite = healthSprites[index];
    }

    // Boss出現時に呼び出す
    public void ShowBossHP()
    {
        if (bossHPBar != null)
            bossHPBar.SetActive(true);
    }
}
