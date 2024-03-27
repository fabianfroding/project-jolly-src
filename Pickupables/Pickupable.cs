using UnityEngine;

// For future:
// How to know if a pikcupable has been picked up. In savedata, save a string scene name + pickupable name.
// When a pickupable is created, check if that scene name + pickupable name exists in savedata, if so destroy the game object immediately.

public class Pickupable : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectPrefab;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player && statusEffectPrefab)
        {
            player.AddStatusEffect(statusEffectPrefab);
            Destroy(gameObject);
        }
    }
}
