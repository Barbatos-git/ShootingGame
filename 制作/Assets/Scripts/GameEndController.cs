using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameEndController : MonoBehaviour
{
    public Image blackFadeImage;
    public TextMeshProUGUI endMessageText;
    public float fadeDuration = 2f;
    public float textDelay = 0.5f;
    public float textFadeOutTime = 0.5f;
    public float messageDisplayTime = 2f;

    private bool isEnding = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(bool playerWon)
    {
        if (isEnding) return;
        isEnding = true;

        string message = playerWon ? "Congratulations!" : "Game Over";
        Color messageColor = playerWon ? Color.yellow : Color.red;

        Sequence seq = DOTween.Sequence();

        seq.Append(blackFadeImage.DOFade(0.7f, fadeDuration));

        seq.AppendInterval(textDelay);
        endMessageText.text = message;
        endMessageText.color = messageColor;
        endMessageText.alpha = 0f;
        seq.Append(endMessageText.DOFade(1f, 0.5f));

        seq.AppendInterval(messageDisplayTime);
        seq.Append(endMessageText.DOFade(0f, textFadeOutTime));

        seq.AppendCallback(() => {
            SceneFader.Instance?.FadeToScene("EndScene");
        });
    }
}
