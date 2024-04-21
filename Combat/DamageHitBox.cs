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
        IDamageable damageable = collision.GetComponentInChildren<IDamageable>();
        if (damageable == null)
            return;

        damageData.target = collision.gameObject;

        if (owningPlayerPawn && collision.GetComponent<PlayerPawn>())
            return;
        if (owningEnemyPawn && collision.GetComponent<EnemyPawn>())
            return;

        damageable.TakeDamage(damageData);
    }
}
