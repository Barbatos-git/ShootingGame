using UnityEngine;
using DG.Tweening;

public class TitleDropAnimator : MonoBehaviour
{
    public RectTransform titleTransform;
    public Vector2 startPos = new Vector2(0, 800); // 開始位置(画面外)
    public Vector2 endPos = new Vector2(0, 0);     // 目標位置(真ん中)
    public float duration = 1.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (titleTransform != null)
        {
            // 初期位置を設定する
            titleTransform.anchoredPosition = startPos;

            // DOTweenを使ってジャンプアニメーション
            titleTransform.DOAnchorPos(endPos, duration)
                .SetEase(Ease.OutBounce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
