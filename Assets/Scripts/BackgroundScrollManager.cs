using UnityEngine;
using System.Collections.Generic;

public class BackgroundScrollManager : MonoBehaviour
{
    public List<BackgroundLayer> backgroundLayers = new List<BackgroundLayer>();
    public bool isScrolling = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isScrolling) return;

        foreach (var layer in backgroundLayers)
        {
            layer.Scroll(Time.deltaTime);
        }
    }

    public void SetScrolling(bool enable)
    {
        isScrolling = enable;
    }
}
