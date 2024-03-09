using System;

public class StatsPlayer : Stats
{
    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerMaxHealthChanged;

    protected override void Start()
    {
        InvokeOnPlayerMaxHealthChangeEvent();
        SetHealth(maxHealth); // TODO: Get from save data.
    }

    public override void DecreaseHealth(int damageAmount)
    {
        base.DecreaseHealth(damageAmount);
        InvokeOnPlayerHealthChangeEvent();
    }

    public override void IncreaseHealth(int amount)
    {
        base.IncreaseHealth(amount);
        InvokeOnPlayerHealthChangeEvent();
    }

    public override void SetHealth(int value)
    {
        base.SetHealth(value);
        InvokeOnPlayerHealthChangeEvent();
    }

    public override void SetMaxHealth(int value)
    {
        base.SetMaxHealth(value);
        InvokeOnPlayerMaxHealthChangeEvent();
    }

    private void InvokeOnPlayerHealthChangeEvent() => OnPlayerHealthChange?.Invoke(CurrentHealth);
    private void InvokeOnPlayerMaxHealthChangeEvent() => OnPlayerMaxHealthChanged?.Invoke(maxHealth);
}
