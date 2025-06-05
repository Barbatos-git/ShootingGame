using UnityEngine;

public class CardFactory : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] allCardSprites;
    public Sprite backSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject CreateCard(CardData data, Transform parent, bool showFront = true, bool isBoss = false)
    {
        GameObject go = Instantiate(cardPrefab, parent, false);
        var display = go.GetComponent<CardDisplay>();
        display.allCardSprites = allCardSprites;
        display.backSprite = isBoss ? backSprite : null;
        display.SetCard(data, showFront);
        return go;
    }
}
