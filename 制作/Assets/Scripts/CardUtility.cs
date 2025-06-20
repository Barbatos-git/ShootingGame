using System.Collections.Generic;
using UnityEngine;

public static class CardUtility
{
    public static CardData GenerateRandomCard()
    {
        return new CardData
        {
            suit = (CardSuit)Random.Range(0, 4),
            rank = (CardRank)Random.Range(1, 14)
        };
    }

    public static List<CardData> TestPair()
    {
        return new List<CardData>
        {
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Three },
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Three },
            new CardData { suit = CardSuit.Hearts,  rank = CardRank.Seven }
        };
    }

    public static List<CardData> TestStraight()
    {
        return new List<CardData>
        {
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Three },
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Four },
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Five }
        };
    }

    public static List<CardData> TestTriple()
    {
        return new List<CardData>
        {
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Six },
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Six },
            new CardData { suit = CardSuit.Hearts, rank = CardRank.Six }
        };
    }
}
