using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class EndSceneResult : MonoBehaviour
{
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI rankText;
    public float countDuration = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var stats=GameStatsManager.Instance;
        if (stats != null)
        {
            StartCoroutine(CountUp(killsText, 0, stats.totalKills, countDuration, "Kills: "));
            StartCoroutine(CountUp(scoreText, 0, stats.totalScore, countDuration, "Score: "));
            timeText.text = "Time: " + FormatTime(stats.totalTime);

            string rank = GetRank(stats.totalScore, stats.totalTime);
            rankText.text = rank;
            if (rank == "S" || rank == "SS" || rank == "SSS")
            {
                ShowFlashRank(rank);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneFader.Instance?.FadeToScene("TitleScene");
        }
    }

    string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60);
        int sec = Mathf.FloorToInt(seconds % 60);
        return $"{min:D2}:{sec:D2}";
    }

    IEnumerator CountUp(TextMeshProUGUI targetText, int from, int to, float duration, string prefix = "")
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(from, to, progress));
            targetText.text = prefix + currentValue;
            yield return null;
        }
        targetText.text = prefix + to;
    }

    string GetRank(int score, float time)
    {
        if (time <= 0f) return "D"; 
        float efficiency = score / time;

        if (efficiency >= 20f) return "SSS";
        else if (efficiency >= 18f) return "SS";
        else if (efficiency >= 16f) return "S";
        else if (efficiency >= 14f) return "A";
        else if (efficiency >= 12f) return "B";
        else if (efficiency >= 10f) return "C";
        else return "D";
    }

    void ShowFlashRank(string rank)
    {
        rankText.text = rank;
        rankText.color = Color.yellow;
        rankText.alpha = 0f;
        rankText.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(rankText.DOFade(1f, 0.3f));
        seq.Join(rankText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        seq.AppendInterval(0.5f);
    }
}
