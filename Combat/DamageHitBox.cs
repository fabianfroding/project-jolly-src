using System.Collections.Generic;
using UnityEngine;

public class DamageHitBox : MonoBehaviour
{
    [SerializeField] private Types.DamageData damageData;
    [SerializeField] private LayerMask affectedLayerMask; // TODO: Consider moving this to damage data.

    private EnemyPawn owningEnemyPawn;
    private PlayerPawn owningPlayerPawn;

    private Collider2D hitBoxCollider2D;
    private List<Collider2D> cachedHitColliders; // Used to filter out already damaged targets.

    private void Awake()
    {
        owningEnemyPawn = GetComponentInParent<EnemyPawn>();
        owningPlayerPawn = GetComponentInParent<PlayerPawn>();
        hitBoxCollider2D = GetComponent<Collider2D>();
        cachedHitColliders = new();
    }

    private void OnEnable()
    {
        DamageOverlappingTargets();
        GameFunctionLibrary.PlayRandomAudioAtPosition(damageData.audioClips, transform.position);
    }

    private void OnDisable()
    {
        cachedHitColliders.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsFriendlyTarget(collision))
            return;
        DamageCollider(collision);
    }

    private void DamageCollider(Collider2D collision)
    {
        if (cachedHitColliders.Contains(collision))
            return;
        IDamageable damageable = collision.GetComponentInChildren<IDamageable>();
        if (damageable == null)
            return;
        damageData.target = collision.gameObject;
        damageable.TakeDamage(damageData);
        cachedHitColliders.Add(collision);
    }

    private bool IsFriendlyTarget(Collider2D collision)
    {
        if (owningPlayerPawn && collision.GetComponent<PlayerPawn>())
            return true;
        if (owningEnemyPawn && collision.GetComponent<EnemyPawn>())
            return true;
        return false;
    }

    private void DamageOverlappingTargets()
    {
        if (!hitBoxCollider2D)
            return;

        List<Collider2D> colliders = new();
        ContactFilter2D contactFilter = new()
        {
            useLayerMask = true,
            layerMask = affectedLayerMask
        };
        Physics2D.OverlapCollider(hitBoxCollider2D, contactFilter, colliders);

        foreach (Collider2D collider in colliders)
            DamageCollider(collider);
    }
}
