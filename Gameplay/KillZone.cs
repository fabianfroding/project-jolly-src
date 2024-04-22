using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private Types.DamageData damageData;

    private void Awake()
    {
        damageData.source = gameObject;
        damageData.isKillZone = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageData.target = collision.gameObject;
            damageable.TakeDamage(damageData);
        }
    }
}
