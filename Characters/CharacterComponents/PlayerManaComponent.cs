using System;
using System.Collections;
using UnityEngine;

public class PlayerManaComponent : CoreComponent
{
    [SerializeField] private int manaGainedPerAttack;
    [SerializeField] private float manaTickInterval = 0.5f;
    [SerializeField] private int manaLostPerTick = 2;

    [SerializeField] private int playerMana;

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
                PlayerCharacter playerCharacter = (PlayerCharacter)componentOwner;
                if (playerCharacter)
                {
                    if (playerCharacter.InAltForm())
                    {
                        float altFormDur = playerCharacter.GetPlayerStateDataAlt().altFormDuration.GetCurrentValue();
                        float elapsed = Time.time - timeAtManaFilled;
                        playerMana = (int)(Mathf.Clamp01((altFormDur - elapsed) / altFormDur) * 100f);
                    }
                    else
                    {
                        playerMana = Mathf.Clamp(playerMana - manaLostPerTick, 0, 100);
                    }
                }
            }
        }
    }

    private void OnDealtDamage()
    {
        if (manaFilled)
            return;

        // TODO: Look for a way to decouple PlayerPawn <-> PlayermanaComponent.
        PlayerCharacter playerCharacter = (PlayerCharacter)componentOwner;
        if (playerCharacter)
        {
            if (playerCharacter.InAltForm())
                return;
        }

        playerMana = Mathf.Clamp(playerMana + manaGainedPerAttack, 0, 100);
        if (playerMana >= 100)
        {
            manaFilled = true;
            StartCoroutine(ManaFilled());
        }
    }

    private IEnumerator ManaFilled()
    {
        yield return new WaitForSeconds(1.5f);
        OnPlayerManaFilled?.Invoke();
        manaFilled = false;
        timeAtManaFilled = Time.time;
    }

}
