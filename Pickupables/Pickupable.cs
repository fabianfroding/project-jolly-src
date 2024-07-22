using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickupable : MonoBehaviour
{
    [SerializeField] private GameObject statusEffectPrefab;

    protected virtual void Awake()
    {
        if (SaveManager.HasPickupableBeenPickedUp(GetPickupableUniqueName()))
            Destroy(gameObject);
    }

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

    protected string GetPickupableUniqueName() => SceneManager.GetActiveScene().name + gameObject.name;
}
