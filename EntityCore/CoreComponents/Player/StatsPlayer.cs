using System;

public class StatsPlayer : Stats
{
    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerMaxHealthChanged;

    protected override void Start()
    {
        InvokeOnPlayerMaxHealthChangeEvent();
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

    public override void SetMaxHealth(int value)
    {
        base.SetMaxHealth(value);
        InvokeOnPlayerMaxHealthChangeEvent();
    }

    private void InvokeOnPlayerHealthChangeEvent() => OnPlayerHealthChange?.Invoke(CurrentHealth);
    private void InvokeOnPlayerMaxHealthChangeEvent() => OnPlayerMaxHealthChanged?.Invoke(maxHealth);
}
