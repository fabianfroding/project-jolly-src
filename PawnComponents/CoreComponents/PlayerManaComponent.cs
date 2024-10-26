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

    private float timeAtManaFilled;

    public static event Action OnPlayerManaFilled;

    private bool manaFilled = false;

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
            if (componentOwner && componentOwner.IsAlive() && !manaFilled)
            {
                PlayerPawn playerPawn = (PlayerPawn)componentOwner;
                if (playerPawn)
                {
                    if (playerPawn.InAltForm())
                    {
                        float altFormDur = playerPawn.GetPlayerStateDataAlt().altFormDuration.GetCurrentValue();
                        float elapsed = Time.time - timeAtManaFilled;
                        playerMana.Value = (int)(Mathf.Clamp01((altFormDur - elapsed) / altFormDur) * 100f);
                        Debug.Log(playerMana.Value);
                    }
                    else
                    {
                        playerMana.Value = Mathf.Clamp(playerMana.Value - manaLostPerTick, 0, 100);
                    }

                    if (OnPlayerManaChangedGameEvent)
                        OnPlayerManaChangedGameEvent.Raise();
                }
            }
        }
    }

    private void OnDealtDamage()
    {
        if (manaFilled)
            return;

        // TODO: Look for a way to decouple PlayerPawn <-> PlayermanaComponent.
        PlayerPawn playerPawn = (PlayerPawn)componentOwner;
        if (playerPawn)
        {
            if (playerPawn.InAltForm())
                return;
        }

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
        if (OnPlayerManaChangedGameEvent)
            OnPlayerManaChangedGameEvent.Raise();
        OnPlayerManaFilled?.Invoke();
        manaFilled = false;
        timeAtManaFilled = Time.time;
    }

}
