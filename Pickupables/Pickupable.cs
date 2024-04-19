using UnityEngine;

// For future:
// How to know if a pikcupable has been picked up. In savedata, save a string scene name + pickupable name.
// When a pickupable is created, check if that scene name + pickupable name exists in savedata, if so destroy the game object immediately.

public class Pickupable : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectPrefab;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPawn player = collision.gameObject.GetComponent<PlayerPawn>();
        if (player && statusEffectPrefab)
        {
            Combat combatComponent = player.GetComponentInChildren<Combat>();
            if (combatComponent)
                combatComponent.AddStatusEffect(statusEffectPrefab);

            Destroy(gameObject);
        }
    }
}
