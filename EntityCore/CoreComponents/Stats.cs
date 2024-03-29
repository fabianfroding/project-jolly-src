using System;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] protected int maxHealth;

    [Tooltip("The amount of hits/damage before the entity can get stunned.")]
    [SerializeField] protected int stunResistance = 3;

    public int CurrentHealth { get; private set; }

    public event Action OnHealthDepleted;
    
    public virtual void OnDealtDamage() {} // Make this into a delegate?

    public virtual bool IsAlive() => CurrentHealth > 0;

    public virtual void DecreaseHealth(int damageAmount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
        if (!IsAlive()) { OnHealthDepleted?.Invoke(); }
    }

    public virtual void IncreaseHealth(int amount) => CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);

    public virtual void SetHealth(int value) => CurrentHealth = Mathf.Max(1, maxHealth);

    public virtual int GetMaxHealth() => maxHealth;
    public virtual void SetMaxHealth(int value) => maxHealth = Mathf.Max(1, value);

    public virtual int GetStunResistance() => stunResistance;
}
