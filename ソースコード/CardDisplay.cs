using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Image cardImage;
    public Sprite[] allCardSprites;
    public Sprite backSprite;
    public CardData cardData;
    public bool showFront = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (cardImage == null)
        {
            cardImage = GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCard(CardData data, bool showFace = true)
    {
        cardData = data;
        showFront = showFace;

        if (cardImage == null) cardImage = GetComponent<Image>();
        if (!showFront && backSprite != null)
        {
            cardImage.sprite = backSprite;
            return;
        }

        int index = cardData.GetSpriteIndex();
        cardImage.sprite = allCardSprites[index];
    }

    public void Flip(bool faceUp)
    {
        showFront = faceUp;
        SetCard(cardData, faceUp);
    }
}
