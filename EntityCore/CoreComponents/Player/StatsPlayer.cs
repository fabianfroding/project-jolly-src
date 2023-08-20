using System;
using System.Collections;
using UnityEngine;

public class StatsPlayer : Stats
{
    [Header("Mana")]
    [SerializeField] protected int maxMana;
    [SerializeField] protected float manaRegenTickInterval = 0.5f;
    [SerializeField] protected float manaRegenPerTick = -0.5f;
    [SerializeField] protected float manaRegenPerAttack = 8f;
    [SerializeField] protected float manaRegenPerDamageTaken = 12f;
    public float currentMana { get; protected set; }

    public static event Action OnPlayerHealthChange;
    public static event Action<float> OnPlayerManaChange;

    #region Getters & Setters
    public float GetManaRegenPerDamageTaken() { return manaRegenPerDamageTaken; }
    #endregion

    protected void Start()
    {
        StartCoroutine(PlayerManaTick());
    }

    private void OnDestroy()
    {
        StopCoroutine(PlayerManaTick());
    }

    public override void OnDealtDamage()
    {
        base.OnDealtDamage();
        IncreaseMana(manaRegenPerAttack);
    }

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

    public override void SetMaxHealth(int value)
    {
        base.SetMaxHealth(value);
        InvokeOnPlayerHealthChangeEvent();
    }

    public void DecreaseMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana - amount, 0, 100);
        InvokeOnPlayerManaChangeEvent();
    }

    public void IncreaseMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, 100);
        InvokeOnPlayerManaChangeEvent();
    }

    private void InvokeOnPlayerHealthChangeEvent()
    {
        OnPlayerHealthChange?.Invoke();
    }

    private void InvokeOnPlayerManaChangeEvent()
    {
        OnPlayerManaChange?.Invoke(currentMana);
    }

    private IEnumerator PlayerManaTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(manaRegenTickInterval);
            DecreaseMana(1);
        }
    }
}
