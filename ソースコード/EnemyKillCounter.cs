using UnityEngine;
using System.Collections.Generic;

public class EnemyKillCounter : MonoBehaviour
{
    public static EnemyKillCounter Instance;

    public int killCount = 0;
    public int requiredKills = 40;
    public int cardRewardInterval = 10;
    public BossSummoner bossSummoner;
    private bool bossSummoned = false;

    public PlayerCardManager playerManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerManager == null) playerManager = FindObjectOfType<PlayerCardManager>();
        if (bossSummoner == null) bossSummoner = FindObjectOfType<BossSummoner>();
    }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterKill()
    {
        killCount++;
        if (!bossSummoned && killCount >= requiredKills)
        {
            bossSummoned = true;
            bossSummoner?.StartBossSequence();
        }

        if (killCount % cardRewardInterval == 0 && killCount < requiredKills)
        {
            CardData card = GenerateRandomCard();
            playerManager.AddCard(card);
            Debug.Log($"[プレーヤー発牌] 撃殺数: {killCount} → 発一枚 {card}");
        }
    }

    CardData GenerateRandomCard()
    {
        CardSuit suit = (CardSuit)Random.Range(0, 4);
        CardRank rank = (CardRank)Random.Range(1, 14);
        return new CardData { suit = suit, rank = rank };
    }
}
