using System.Collections.Generic;
using System.Linq;

public enum HandType { HighCard, Pair, Straight, Triple }

public class CardEvaluator
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static HandType Evaluate(List<CardData> hand)
    {
        var ranks = hand.Select(c => (int)c.rank).OrderBy(x => x).ToList();
        bool triple = hand.All(c => c.rank == hand[0].rank);
        bool pair = hand.GroupBy(c => c.rank).Any(g => g.Count() == 2);
        bool straight = ranks[2] == ranks[1] + 1 && ranks[1] == ranks[0] + 1;

        if (triple) return HandType.Triple;
        if (straight) return HandType.Straight;
        if (pair) return HandType.Pair;
        return HandType.HighCard;
    }

    public static int Compare(List<CardData> a, List<CardData> b)
    {
        HandType typeA = Evaluate(a);
        HandType typeB = Evaluate(b);
        if (typeA != typeB) return typeA.CompareTo(typeB);

        var sortedA = a.OrderByDescending(c => (int)c.rank).ToList();
        var sortedB = b.OrderByDescending(c => (int)c.rank).ToList();
        for (int i = 0; i < 3; i++)
        {
            int diff = ((int)sortedA[i].rank).CompareTo((int)sortedB[i].rank);
            if (diff != 0) return diff;
        }
        return 0;
    }
}
