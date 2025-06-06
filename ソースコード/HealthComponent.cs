using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    public event Action<int, int> OnHealthChanged; // 現在HP, 最大HP
    public UnityEvent OnDeath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        OnHealthChanged?.Invoke(currentHP, maxHP);

        if (currentHP <= 0)
        {
            OnDeath?.Invoke();
        }

        Debug.Log("現在HP：" + currentHP);
    }

    public int GetCurrentHP() => currentHP;
    public int GetMaxHP() => maxHP;
}
