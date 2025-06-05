using UnityEngine;

public class EffectController : MonoBehaviour
{
    [HideInInspector]
    public BossSummoner summoner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 动画事件调用这个方法
    public void OnEffectAnimationEnd()
    {
        summoner?.OnEffectAnimationEnd();
    }
}
