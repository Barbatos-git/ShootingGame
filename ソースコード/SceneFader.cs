using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    public Image blackFadeImage;
    public float fadeDuration = 0.8f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeIn();
        }
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToScene(string sceneName)
    {
        blackFadeImage.raycastTarget = true;
        blackFadeImage.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }

    public void FadeIn()
    {
        blackFadeImage.color = new Color(0, 0, 0, 1);
        blackFadeImage.DOFade(0f, fadeDuration).OnComplete(() =>
        {
            blackFadeImage.raycastTarget = false;
        });
    }
}
