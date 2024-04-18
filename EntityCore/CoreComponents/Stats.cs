using System;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] protected int maxHealth;

    public int CurrentHealth { get; private set; }

    public event Action<int> OnHealthChange;
    public event Action<int> OnMaxHealthChanged;
    public event Action OnHealthDepleted;

    protected override void Start()
    {
        OnMaxHealthChanged?.Invoke(maxHealth);
        SetHealth(maxHealth);
    }

    public virtual bool IsAlive() => CurrentHealth > 0;

    public virtual void DecreaseHealth(int damageAmount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
        if (IsAlive())
            OnHealthChange?.Invoke(CurrentHealth);
        else
            OnHealthDepleted?.Invoke();
    }

    public virtual void IncreaseHealth(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public virtual void SetHealth(int value)
    {
        CurrentHealth = Mathf.Max(1, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public virtual int GetMaxHealth() => maxHealth;
    public virtual void SetMaxHealth(int value)
    {
        maxHealth = Mathf.Max(1, value);
        OnMaxHealthChanged?.Invoke(maxHealth);
    }
}
