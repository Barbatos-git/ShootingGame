using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BossCardManager : MonoBehaviour
{
    public Transform[] handSlots = new Transform[3];
    public Transform reserveArea;
    public CardFactory cardFactory;
    public Transform playerReserveTarget; // カードをプレイヤーのカード受ける場所へ送る
    public PlayerCardManager playerCardManager; // プレイヤーカード管理

    public List<CardData> handCards = new List<CardData>();
    private List<GameObject> handObjects = new List<GameObject>();
    private CardData reserveCard = null;
    private GameObject reserveObject = null;

    private float timer = 0f;
    public float interval = 20f;

    public bool pair = false;
    public bool straight = false;
    public bool triple = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 自動的にgameobjectを探す
        if (playerCardManager == null)
        {
            playerCardManager = FindObjectOfType<PlayerCardManager>();
        }
        if (cardFactory == null)
        {
            cardFactory = FindObjectOfType<CardFactory>();
        }
        if (playerReserveTarget == null)
        {
            var target = GameObject.Find("PlayerReserveArea");
            if (target != null) playerReserveTarget = target.transform;
        }
        if (handSlots.All(h => h == null))
        {
            for (int i = 0; i < 3; i++)
            {
                var obj = GameObject.Find($"BossHandPosition_{i}");
                if (obj != null) handSlots[i] = obj.transform;
            }
        }
        if (reserveArea == null)
        {
            var rs = GameObject.Find("BossReserveArea");
            if (rs != null) reserveArea = rs.transform;
        }

        // Boss登場する時に3枚カードをゲットする
        if (pair)
        {
            var card = CardUtility.TestPair();
            for (int i = 0; i < 3; i++)
            {
                AddCard(card[i]);
                Debug.Log($"第{i + 1}枚: {card[i].suit} {card[i].rank}");
            }
        }
        else if (straight)
        {
            var card = CardUtility.TestStraight();
            for (int i = 0; i < 3; i++)
            {
                AddCard(card[i]);
                Debug.Log($"第{i + 1}枚: {card[i].suit} {card[i].rank}");
            }
        }
        else if (triple)
        {
            var card = CardUtility.TestTriple();
            for (int i = 0; i < 3; i++)
            {
                AddCard(card[i]);
                Debug.Log($"第{i + 1}枚: {card[i].suit} {card[i].rank}");
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                var card = CardUtility.GenerateRandomCard();
                AddCard(card);
                Debug.Log($"第{i + 1}枚: {card.suit} {card.rank}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;

            // Bossのカード受ける場所は空いてる → Bossにカードを配る
            if (reserveCard == null)
            {
                AddCard(CardUtility.GenerateRandomCard());
            }

            // プレイヤーのカード受ける場所は空いてる → プレイヤーにカードを配る
            if (playerCardManager != null && playerCardManager.HasReserve() == false)
            {
                playerCardManager.AddCard(CardUtility.GenerateRandomCard());
                Debug.Log("プレイヤーは新しいカードをゲットする");
            }
        }
    }

    public void AddCard(CardData data)
    {
        if (handCards.Count < 3)
        {
            handCards.Add(data);
            GameObject go = cardFactory.CreateCard(data, handSlots[handCards.Count - 1], false, true);
            go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            handObjects.Add(go);
        }
        else if (reserveCard == null)
        {
            reserveCard = data;
            reserveObject = cardFactory.CreateCard(data, reserveArea, true, true);
            reserveObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            // ReserveCardSlot自動破壊を追加する
            var slot = reserveObject.AddComponent<ReserveCardSlot>();
            slot.cardData = data;
            slot.OnExpired = obj => {
                reserveCard = null;
                reserveObject = null;
                Destroy(obj.gameObject);
            };
        }
    }

    public void OnPlayerPlayCard(CardData playerCard)
    {
        if (handCards.Count == 0) return;

        CardData best = handCards.OrderByDescending(c => (int)c.rank).First();

        if ((int)best.rank > (int)playerCard.rank)
        {
            PlayCard(best);
        }
        else
        {
            Debug.Log("Bossはカードを出さない");
        }
    }

    void PlayCard(CardData card)
    {
        int index = handCards.IndexOf(card);
        if (index >= 0)
        {
            GameObject go = handObjects[index];
            go.transform.SetParent(playerReserveTarget, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            go.GetComponent<CardDisplay>().Flip(true);

            handCards.RemoveAt(index);
            handObjects.RemoveAt(index);

            if (reserveCard != null)
            {
                AddCard(reserveCard);
                Destroy(reserveObject);
                reserveCard = null;
                reserveObject = null;
            }

            Debug.Log("Bossはカードを出す: " + card);
        }
    }
}
