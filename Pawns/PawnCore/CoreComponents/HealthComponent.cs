using System;
using UnityEngine;

public class HealthComponent : CoreComponent
{
    [SerializeField] protected int maxHealth;

    public int CurrentHealth { get; private set; }

    private bool invulnerable = false; // TODO: This can be made into a Dictionary if there are multiple invulnerability-granting sources.
    private InvulnerabilityIndication invulnerabilityIndication;

    public event Action<int> OnHealthChange;
    public event Action<int> OnMaxHealthChanged;
    public event Action OnHealthDepleted;

    protected override void Awake()
    {
        base.Awake();
        invulnerabilityIndication = GetComponent<InvulnerabilityIndication>();
    }

    protected override void Start()
    {
        OnMaxHealthChanged?.Invoke(maxHealth);
        SetHealth(maxHealth);
    }

    public bool IsAlive() => CurrentHealth > 0;
    public bool IsInvulnerable() => invulnerable;

    public void DecreaseHealth(int damageAmount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
        if (IsAlive())
            OnHealthChange?.Invoke(CurrentHealth);
        else
            OnHealthDepleted?.Invoke();

        if (invulnerabilityIndication)
            invulnerabilityIndication.StartFlash();
    }

    public void IncreaseHealth(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public void SetHealth(int value)
    {
        CurrentHealth = Mathf.Max(1, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public int GetMaxHealth() => maxHealth;
    public void SetMaxHealth(int value)
    {
        maxHealth = Mathf.Max(1, value);
        OnMaxHealthChanged?.Invoke(maxHealth);
    }

    public void SetInvulnerable(bool newInvulnerable)
    {
        invulnerable = newInvulnerable;
        if (!invulnerable && invulnerabilityIndication)
            invulnerabilityIndication.EndFlash();
    }
}
