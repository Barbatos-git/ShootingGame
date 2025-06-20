using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;
    public int totalKills = 0;
    public int totalScore = 0;
    public float totalTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetStats()
    { 
        totalKills = 0;
        totalScore = 0;
        totalTime = 0f;
    }
}
