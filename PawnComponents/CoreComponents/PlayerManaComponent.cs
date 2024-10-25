using System;
using System.Collections;
using UnityEngine;

public class PlayerManaComponent : CoreComponent
{
    [SerializeField] private int manaGainedPerAttack;
    [SerializeField] private float manaTickInterval = 0.5f;
    [SerializeField] private int manaLostPerTick = 2;

    [SerializeField] private SOIntVariable playerMana;
    [SerializeField] private SOGameEvent OnPlayerManaChangedGameEvent;

    public static event Action OnPlayerManaFilled;

    private bool manaFilled = false;

    protected override void Start()
    {

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
        if (manaFilled)
            return;

        playerMana.Value = Mathf.Clamp(playerMana.Value + manaGainedPerAttack, 0, 100);
        if (playerMana.Value >= 100)
        {
            manaFilled = true;
            StartCoroutine(ManaFilled());
        }
        if (OnPlayerManaChangedGameEvent)
            OnPlayerManaChangedGameEvent.Raise();
    }

    private IEnumerator ManaFilled()
    {
        yield return new WaitForSeconds(1.5f);
        playerMana.Value = 0;
        if (OnPlayerManaChangedGameEvent)
            OnPlayerManaChangedGameEvent.Raise();
        OnPlayerManaFilled?.Invoke();
        manaFilled = false;
    }

}
