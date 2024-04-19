using UnityEngine;

public class DamageHitBox : MonoBehaviour
{
    [SerializeField] private Types.DamageData damageData;

    private EnemyPawn owningEnemyPawn;
    private PlayerPawn owningPlayerPawn;

    private void Awake()
    {
        owningEnemyPawn = GetComponentInParent<EnemyPawn>();
        owningPlayerPawn = GetComponentInParent<PlayerPawn>();
    }

    private void OnEnable()
    {
        if (damageData.sfxPrefab)
        {
            GameObject sfxInstance = GameObject.Instantiate(damageData.sfxPrefab);
            sfxInstance.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthComponent healthComponent;
        Combat combat;

        EnemyPawn collidingEnemyPawn = collision.GetComponent<EnemyPawn>();
        if (owningPlayerPawn && collidingEnemyPawn)
        {
            healthComponent = collidingEnemyPawn.GetComponentInChildren<HealthComponent>();
            damageData.target = collision.gameObject;
            combat = collidingEnemyPawn.GetComponentInChildren<Combat>();
            if (combat)
                combat.TakeDamage(damageData);
        }

        PlayerPawn collidingPlayerPawn = collision.GetComponent<PlayerPawn>();
        if (owningEnemyPawn && collidingPlayerPawn)
        {
            healthComponent = collidingEnemyPawn.GetComponentInChildren<HealthComponent>();
            damageData.target = collision.gameObject;
            combat = collidingEnemyPawn.GetComponentInChildren<Combat>();
            if (combat)
                combat.TakeDamage(damageData);
        }
    }
}
