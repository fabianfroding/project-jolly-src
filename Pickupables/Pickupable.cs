using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickupable : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectPrefab; // TODO: Make subclass for status effect pickupable.

    protected virtual void Start()
    {
        if (SaveManager.HasPickupableBeenPickedUp(GetPickupableUniqueName()))
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player && statusEffectPrefab)
        {
            Combat combatComponent = player.GetComponentInChildren<Combat>();
            if (combatComponent)
                combatComponent.AddStatusEffect(statusEffectPrefab);

            Destroy(gameObject);
        }
    }

    protected string GetPickupableUniqueName() => SceneManager.GetActiveScene().name + gameObject.name;
}
