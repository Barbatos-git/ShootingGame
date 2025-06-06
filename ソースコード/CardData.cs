public enum CardSuit { Hearts, Diamonds, Clubs, Spades }
public enum CardRank { A = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K }

[System.Serializable]
public class CardData
{
    public CardSuit suit;
    public CardRank rank;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetSpriteIndex()
    {
        return ((int)suit) * 13 + ((int)rank - 1);
    }

    public override string ToString()
    {
        return $"{suit} {rank}";
    }
}
