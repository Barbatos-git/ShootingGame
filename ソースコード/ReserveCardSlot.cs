using UnityEngine;
using System.Collections;

public class ReserveCardSlot : MonoBehaviour
{
    public CardData cardData;
    public System.Action<ReserveCardSlot> OnExpired;

    private float timer = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AutoDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(timer);
        OnExpired?.Invoke(this);
        Destroy(gameObject);
    }
}
