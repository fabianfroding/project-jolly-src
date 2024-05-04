using System.Collections;
using UnityEngine;

public class PlayerManaComponent : CoreComponent
{
    [SerializeField] private int manaGainedPerAttack;
    [SerializeField] private float manaTickInterval = 0.5f;
    [SerializeField] private int manaLostPerTick = 2;

    [SerializeField] private SOIntVariable playerMana;
    [SerializeField] public SOIntVariable playerManaCharges;
    [SerializeField] private SOIntVariable playerMaxManaCharges;

    [SerializeField] private SOGameEvent OnPlayerManaChangedGameEvent;
    [SerializeField] private SOGameEvent OnPlayerManaChargesChangedGameEvent;
    [SerializeField] private SOGameEvent OnPlayerMaxManaChargesChangedGameEvent;

    protected override void Start()
    {
        if (OnPlayerMaxManaChargesChangedGameEvent)
            OnPlayerMaxManaChargesChangedGameEvent.Raise();
        SetManaCharges(0); // TODO: This should be fetched from save data.
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
                playerMana.Value = Mathf.Clamp(playerMana.Value - manaLostPerTick, 0, 100);
                if (OnPlayerManaChangedGameEvent)
                    OnPlayerManaChangedGameEvent.Raise();
            }
        }
    }

    private void OnDealtDamage()
    {
        playerMana.Value = Mathf.Clamp(playerMana.Value + manaGainedPerAttack, 0, 100);
        if (playerMana.Value >= 100 && playerManaCharges.Value < playerMaxManaCharges.Value)
        {
            playerManaCharges.Value = Mathf.Clamp(playerManaCharges.Value + 1, 0, playerMaxManaCharges.Value);
            if (OnPlayerManaChargesChangedGameEvent)
                OnPlayerManaChargesChangedGameEvent.Raise();
            playerMana.Value = 0;
        }
        if (OnPlayerManaChangedGameEvent)
            OnPlayerManaChangedGameEvent.Raise();
    }

    private void SetManaCharges(int value)
    {
        playerManaCharges.Value = value;
        if (OnPlayerManaChargesChangedGameEvent)
            OnPlayerManaChargesChangedGameEvent.Raise();
    }

    public void ConsumeManaCharge()
    {
        playerManaCharges.Value = Mathf.Clamp(playerManaCharges.Value - 1, 0, playerMaxManaCharges.Value);
        if (OnPlayerManaChargesChangedGameEvent)
            OnPlayerManaChargesChangedGameEvent.Raise();
    }
}
