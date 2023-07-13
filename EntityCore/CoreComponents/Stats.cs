using System;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField] protected int maxHealth;
    public int currentHealth { get; protected set; }

    [Tooltip("The amount of hits/damage before the entity can get stunned.")]
    [SerializeField] protected int stunResistance = 3;

    public event Action OnHealthDepleted;

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
    }

    public virtual void OnDealtDamage() {}

    public virtual void DecreaseHealth(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnHealthDepleted?.Invoke();
        }
    }

    public virtual void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public virtual void SetHealth(int value)
    {
        if (value > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (value < 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth = value;
        }
    }

    public virtual int GetMaxHealth()
    {
        return maxHealth;
    }

    public virtual void SetMaxHealth(int value)
    {
        if (maxHealth <= 0)
        {
            maxHealth = 1;
        }
        else
        {
            maxHealth = value;
        }
    }

    public virtual int GetStunResistance()
    {
        return stunResistance;
    }
}
