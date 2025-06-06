using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public Image targetImage; // 表示用のImageコンポーネント
    public Sprite[] healthSprites; // 0 = 空，11 = いっぱい

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BindTo(HealthComponent player)
    {
        player.OnHealthChanged += UpdateHP;
        UpdateHP(player.GetCurrentHP(), player.GetMaxHP());
    }

    public void UpdateHP(int current, int max)
    {
        float percent = (float)current / max;
        int index = Mathf.Clamp(Mathf.RoundToInt(percent * (healthSprites.Length - 1)), 0, healthSprites.Length - 1);
        targetImage.sprite = healthSprites[index];
    }
}
