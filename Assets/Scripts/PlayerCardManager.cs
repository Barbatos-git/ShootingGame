using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerCardManager : MonoBehaviour
{
    public Transform[] handPositions = new Transform[3];
    public Transform reserveArea;
    public Transform bossReserveArea; // Bossのカード受ける場所
    public CardFactory cardFactory;
    public int maxHand = 3;

    private CardData[] handCards = new CardData[3];
    private GameObject[] handCardObjects = new GameObject[3];
    private CardData reserveCard = null;
    private GameObject reserveCardObject = null;
    private int selectedIndex = -1;

    public bool pair = false;
    public bool straight = false;
    public bool triple = false;

    private void Start()
    {
        if (pair)
        {
            var card = CardUtility.TestPair();
            for (int i = 0; i < 3; i++)
            {
                AddCard(card[i]);
            }
        }
        else if (straight)
        {
            var card = CardUtility.TestStraight();
            for (int i = 0; i < 3; i++)
            {
                AddCard(card[i]);
            }
        }
        else if (triple)
        {
            var card = CardUtility.TestTriple();
            for (int i = 0; i < 3; i++)
            {
                AddCard(card[i]);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                var card = CardUtility.GenerateRandomCard();
                AddCard(card);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) HandleInput(0);
        if (Input.GetKeyDown(KeyCode.K)) HandleInput(1);
        if (Input.GetKeyDown(KeyCode.L)) HandleInput(2);
    }

    void HandleInput(int index)
    {
        Debug.Log($"キー入力 index = {index}, 手札: {(handCardObjects[index] == null ? "空" : "あり")}, data: {(handCards[index] == null ? "空" : "あり")}");

        if (!IsHandFull() || reserveCard == null)
        {
            return;
        }

        if (index < 0 || index >= maxHand) return;

        if (selectedIndex == index)
        {
            PlayCard(index);
            selectedIndex = -1;
        }
        else
        {
            selectedIndex = index;
            HighlightCard(index);
        }
    }

    void HighlightCard(int index)
    {
        for (int i = 0; i < handCardObjects.Length; i++)
        {
            if (handCardObjects[i] == null) continue;
            var rt = handCardObjects[i].GetComponent<RectTransform>();
            if (rt != null)
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, (i == index ? 30f : 0f));
        }
    }

    void ResetHighlight()
    {
        for (int i = 0; i < handCardObjects.Length; i++)
        {
            if (handCardObjects[i] == null) continue;
            var rt = handCardObjects[i].GetComponent<RectTransform>();
            if (rt != null)
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 0f);
        }
    }

    public bool CanReceiveCard()
    {
        return reserveCard == null && reserveCardObject == null;
    }

    public void AddCard(CardData data)
    {
        if (IsHandFull() && !CanReceiveCard())
        {
            return;
        }

        Debug.Log($"AddCard() 呼び出される：{data.rank} of {data.suit}");

        for (int i = 0; i < maxHand; i++)
        {
            if (handCardObjects[i] == null)
            {
                handCards[i] = data;
                Debug.Log($"handCards[{i}] 代入完了");

                GameObject cardGO = cardFactory.CreateCard(data, handPositions[i], true, false);
                cardGO.transform.SetParent(handPositions[i], false);
                cardGO.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                handCardObjects[i] = cardGO;

                Debug.Log($"handCardObjects[{i}] 代入完了 → GameObject: {cardGO.name}");
                return;
            }
        }

        if (CanReceiveCard())
        {
            reserveCard = data;
            reserveCardObject = cardFactory.CreateCard(data, reserveArea, true, false);
            reserveCardObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            var slot = reserveCardObject.AddComponent<ReserveCardSlot>();
            slot.cardData = data;
            slot.OnExpired = s => {
                reserveCard = null;
                reserveCardObject = null;
                Destroy(s.gameObject);
            };
        }
        else
        {
           
        }
    }

    void PlayCard(int index)
    {
        if (index < 0 || index >= maxHand || handCards[index] == null || handCardObjects[index] == null)
        {
            return;
        }

        CardData played = handCards[index];
        GameObject playedCard = handCardObjects[index];

        playedCard.transform.SetParent(bossReserveArea, false);
        var rt = playedCard.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
        }
        playedCard.GetComponent<CardDisplay>().Flip(true);

        handCardObjects[index] = null;
        handCards[index] = null;

        ResetHighlight();
        TryFillHandFromReserve();

        Debug.Log("プレイヤーはカードを出す: " + played);
    }

    void TryFillHandFromReserve()
    {
        if (reserveCard == null) return;

        for (int i = 0; i < maxHand; i++)
        {
            if (handCardObjects[i] == null)
            {
                AddCard(reserveCard);
                Destroy(reserveCardObject);
                reserveCard = null;
                reserveCardObject = null;
                break;
            }
        }
    }

    bool IsHandFull()
    {
        for (int i = 0; i < maxHand; i++)
        {
            if (handCardObjects[i] == null || handCards[i] == null)
                return false;
        }
        return true;
    }

    public CardData[] GetHandCards()
    {
        return handCards;
    }

    public bool HasReserve() => reserveCard != null && reserveCardObject != null;
}
