using System;
using System.Collections;
using UnityEngine;

public class StatsPlayer : Stats
{
    [SerializeField] private int manaGainedPerAttack;
    [SerializeField] private float manaTickInterval = 0.5f;
    [SerializeField] private int manaLostPerTick = 2;
    [SerializeField] private int maxManaCharges = 2;

    public int CurrentMana { get; private set; }
    public int CurrentManaCharges { get;private set; }

    public MutableFloat movementSpeedModifier;

    public static event Action<int> OnPlayerHealthChange;
    public static event Action<int> OnPlayerMaxHealthChanged;
    public static event Action<int> OnPlayerManaChange;
    public static event Action<int> OnPlayerManaChargesChange;
    public static event Action<int> OnPlayerMaxManaChargesChange;

    protected override void Start()
    {
        OnPlayerMaxHealthChanged?.Invoke(maxHealth);
        OnPlayerMaxManaChargesChange?.Invoke(maxManaCharges);
        SetHealth(maxHealth);
        SetManaCharges(0);

        movementSpeedModifier = new MutableFloat();
    }

    private void OnEnable()
    {
        StartCoroutine(ManaTick());
    }

    private void OnDisable()
    {
        StopCoroutine(ManaTick());
    }

    private IEnumerator ManaTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(manaTickInterval);
            if (IsAlive())
            {
                CurrentMana = Mathf.Clamp(CurrentMana - manaLostPerTick, 0, 100);
                OnPlayerManaChange?.Invoke(CurrentMana);
            }
        }
    }

    public override void OnDealtDamage()
    {
        base.OnDealtDamage();
        CurrentMana = Mathf.Clamp(CurrentMana + manaGainedPerAttack, 0, 100);
        if (CurrentMana >= 100 && CurrentManaCharges < maxManaCharges)
        {
            CurrentManaCharges = Mathf.Clamp(CurrentManaCharges + 1, 0, maxManaCharges);
            OnPlayerManaChargesChange?.Invoke(CurrentManaCharges);
            CurrentMana = 0;
        }
        OnPlayerManaChange?.Invoke(CurrentMana);
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

    public void SetManaCharges(int value)
    {
        CurrentManaCharges = value;
        OnPlayerManaChargesChange?.Invoke(value);
    }
}
