using UnityEngine;

[System.Serializable]
public class BackgroundLayer : MonoBehaviour
{
    public Transform layer1;
    public Transform layer2;
    public float scrollSpeed;
    public float resetY;
    public float startY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Scroll(float deltaTime)
    {
        layer1.Translate(Vector3.down * scrollSpeed * deltaTime);
        layer2.Translate(Vector3.down * scrollSpeed * deltaTime);

        ResetIfNeeded(layer1);
        ResetIfNeeded(layer2);
    }

    private void ResetIfNeeded(Transform t)
    {
        if (t.position.y <= resetY)
        {
            t.position = new Vector3(t.position.x, startY, t.position.z);
        }
    }
}
