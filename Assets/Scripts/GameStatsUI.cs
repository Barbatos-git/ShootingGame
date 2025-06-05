using UnityEngine;
using TMPro;

public class GameStatsUI : MonoBehaviour
{
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI scoreText;

    private int killCount = 0;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        GameStatsManager.Instance.totalTime += Time.deltaTime;
    }

    public void AddScore(int value)
    {
        score += value;
        GameStatsManager.Instance.totalScore = score;
        UpdateUI();
    }

    public void AddKill()
    {
        killCount++;
        GameStatsManager.Instance.totalKills = killCount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (killsText != null)
            killsText.text = "Kills: " + killCount;
    }
}
