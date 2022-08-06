using System;

public class StatsPlayer : Stats
{
    public static event Action OnPlayerHealthChange;

    public override void DecreaseHealth(int amount)
    {
        base.DecreaseHealth(amount);
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
        InvokeOnPlayerHealthChangeEvent();
    }

    private void InvokeOnPlayerHealthChangeEvent()
    {
        OnPlayerHealthChange?.Invoke();
    }
}
