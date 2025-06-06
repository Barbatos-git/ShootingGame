using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class PressStartBlink : MonoBehaviour
{
    public TextMeshProUGUI pressText;
    public float blinkDuration = 0.5f;
    public string gameSceneName = "GameScene";

    private Tween blinkTween;
    private bool transitioning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pressText != null)
        {
            StartBlink();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!transitioning && Input.GetKeyDown(KeyCode.Space))
        {
            transitioning = true;
            StopBlink();

            SceneFader.Instance?.FadeToScene("GameScene");
        }
    }

    void StartBlink()
    {
        blinkTween = pressText.DOFade(0f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void StopBlink()
    {
        if (blinkTween != null && blinkTween.IsActive())
        {
            blinkTween.Kill(); // Tweenを中止する
        }

        pressText.DOFade(1f, 0.2f); // 見えるようにする
    }
}
