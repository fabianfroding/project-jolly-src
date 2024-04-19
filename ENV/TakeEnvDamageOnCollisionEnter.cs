using UnityEngine;

public class TakeEnvDamageOnCollisionEnter : MonoBehaviour
{
    [SerializeField] private Types.DamageData damageData;

    private void Awake()
    {
        damageData.source = gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Instantly kill enemies.
        if (collision.gameObject.GetComponent<EnemyPawn>())
        {
            Death death = collision.gameObject.GetComponentInChildren<Death>();
            if (death)
            {
                death.Die();
                return;
            }
        }

        damageData.target = collision.gameObject;
        IDamageable damageable = collision.gameObject.GetComponentInChildren<IDamageable>();
        damageable?.TakeDamage(damageData);
    }
}
