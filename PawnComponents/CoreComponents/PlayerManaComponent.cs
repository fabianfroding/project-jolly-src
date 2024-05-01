using System;
using System.Collections;
using UnityEngine;

public class PlayerManaComponent : CoreComponent
{
    [SerializeField] private int manaGainedPerAttack;
    [SerializeField] private float manaTickInterval = 0.5f;
    [SerializeField] private int manaLostPerTick = 2;
    [SerializeField] private int maxManaCharges = 2;

    public int CurrentMana { get; private set; }
    public int CurrentManaCharges { get; private set; }

    public static event Action<int> OnPlayerManaChange;
    public static event Action<int> OnPlayerManaChargesChange;
    public static event Action<int> OnPlayerMaxManaChargesChange;

    protected override void Start()
    {
        OnPlayerMaxManaChargesChange?.Invoke(maxManaCharges);
        SetManaCharges(0);
    }

    private void OnEnable()
    {
        StartCoroutine(ManaTick());
        if (componentOwner)
            componentOwner.OnDealtDamage += OnDealtDamage;
    }

    private void OnDisable()
    {
        StopCoroutine(ManaTick());

        if (componentOwner)
            componentOwner.OnDealtDamage -= OnDealtDamage;
    }

    private IEnumerator ManaTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(manaTickInterval);
            if (componentOwner && componentOwner.IsAlive())
            {
                CurrentMana = Mathf.Clamp(CurrentMana - manaLostPerTick, 0, 100);
                OnPlayerManaChange?.Invoke(CurrentMana);
            }
        }
    }

    private void OnDealtDamage()
    {
        CurrentMana = Mathf.Clamp(CurrentMana + manaGainedPerAttack, 0, 100);
        if (CurrentMana >= 100 && CurrentManaCharges < maxManaCharges)
        {
            CurrentManaCharges = Mathf.Clamp(CurrentManaCharges + 1, 0, maxManaCharges);
            OnPlayerManaChargesChange?.Invoke(CurrentManaCharges);
            CurrentMana = 0;
        }
        OnPlayerManaChange?.Invoke(CurrentMana);
    }

    private void SetManaCharges(int value)
    {
        CurrentManaCharges = value;
        OnPlayerManaChargesChange?.Invoke(value);
    }

    public void ConsumeManaCharge()
    {
        CurrentManaCharges = Mathf.Clamp(CurrentManaCharges - 1, 0, maxManaCharges);
        OnPlayerManaChargesChange?.Invoke(CurrentManaCharges);
    }
}
