using System;
using System.Linq;
using UnityEngine;
using static Types;

public class Stats : CoreComponent
{
    [SerializeField] protected int maxHealth;
    public HealthState[] health;

    [Tooltip("The amount of hits/damage before the entity can get stunned.")]
    [SerializeField] protected int stunResistance = 3;

    public event Action OnHealthDepleted;
    public event Action OnHealthChanged;

    protected override void Awake()
    {
        base.Awake();
        health = new HealthState[maxHealth];
    }

    public virtual void OnDealtDamage() {}

    public virtual bool IsAlive()
    {
        if (health.Any(state => state == HealthState.FILLED)) return true;
        return false;
    }

    public virtual void DecreaseHealth(int amount)
    {
        int lastFilledIndex = -1;
        for (int i = health.Length - 1; i >= 0; i--)
        {
            if (health[i] == HealthState.FILLED)
            {
                lastFilledIndex = i;
                break;
            }
        }

        if (lastFilledIndex != -1)
        {
            for (int i = lastFilledIndex; i >= Mathf.Max(0, lastFilledIndex - amount + 1); i--)
            {
                health[i] = HealthState.EMPTY;
                OnHealthChanged?.Invoke();
            }
        }

        if (health.All(state => state == HealthState.EMPTY)) { OnHealthDepleted?.Invoke(); }
    }

    public virtual void IncreaseHealth(int amount)
    {
        for (int i = 0; i < health.Length && amount > 0; i++)
        {
            if (health[i] == HealthState.EMPTY)
            {
                health[i] = HealthState.FILLED;
                amount--;
                OnHealthChanged?.Invoke();
            }
        }
    }

    public virtual void SetHealth(HealthState[] value) { health = value; }

    public virtual int GetMaxHealth() { return maxHealth; }

    public virtual void SetMaxHealth(int value)
    {
        health = new HealthState[value];
        for (int i = 0; i < health.Length; i++)
        {
            health[i] = HealthState.FILLED;
        }
    }

    public virtual int GetStunResistance() { return stunResistance; }
}
