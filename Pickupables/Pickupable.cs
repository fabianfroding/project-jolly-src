using UnityEngine;

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
