using System;

public class StatsPlayer : Stats
{
    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerMaxHealthChanged;

    protected override void Start()
    {
        OnPlayerMaxHealthChanged?.Invoke(maxHealth);
        SetHealth(maxHealth);
    }

    public override void DecreaseHealth(int damageAmount)
    {
        base.DecreaseHealth(damageAmount);
        OnPlayerHealthChange?.Invoke(CurrentHealth);
    }

    public override void IncreaseHealth(int amount)
    {
        base.IncreaseHealth(amount);
        OnPlayerHealthChange?.Invoke(CurrentHealth);
    }

    public override void SetHealth(int value)
    {
        base.SetHealth(value);
        OnPlayerHealthChange?.Invoke(CurrentHealth);
    }

    public override void SetMaxHealth(int value)
    {
        base.SetMaxHealth(value);
        OnPlayerMaxHealthChanged?.Invoke(maxHealth);
    }
}
