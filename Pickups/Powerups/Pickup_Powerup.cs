using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Powerup : Pickup
{
    [Tooltip("The particle system prefab that is active while the player is obtaining the powerup.")]
    [SerializeField] private GameObject sfxObtainingPrefab;
    [Tooltip("The particle system prefab that is spawned when the player finishes obtaining the powerup.")]
    [SerializeField] private GameObject sfxObtainedPrefab;
    [SerializeField] private List<string> abilitiesUnlocked;

    public static event Action<string> OnPickupPowerup;

    private GameObject player;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            player = collision.gameObject;
        }
        base.OnTriggerEnter2D(collision);
    }

    protected override void GetPickup()
    {
        InstantiateOnPickupSound();
        GameObject sfxObtaining = Instantiate(sfxObtainingPrefab);
        sfxObtaining.transform.position = player.transform.position;
        sfxObtaining.transform.parent = player.transform;

        ParticleSystem sfxObtainingParticleSystem = sfxObtaining.GetComponent<ParticleSystem>();
        StartCoroutine(ObtainPowerup(sfxObtainingParticleSystem != null ? sfxObtainingParticleSystem.main.duration * 1.8f : 0f));
    }

    private IEnumerator ObtainPowerup(float delay)
    {
        yield return new WaitForSeconds(delay);

        InstantiateSFXObtained();
        OnPickupPowerup?.Invoke(gameObject.name);

        if (player)
        {
            PlayerAbilityManager playerAbilityManager = player.GetComponent<PlayerAbilityManager>();
            if (playerAbilityManager)
            {
                foreach (string abilityUnlocked in abilitiesUnlocked)
                {
                    playerAbilityManager.EnableAbility(abilityUnlocked);
                }
            }
        }

        Destroy(gameObject);
    }

    private void InstantiateSFXObtained()
    {
        if (sfxObtainedPrefab != null)
        {
            GameObject sfxBurst = Instantiate(sfxObtainedPrefab);
            sfxBurst.transform.position = player.transform.position;
        }
    }
}
