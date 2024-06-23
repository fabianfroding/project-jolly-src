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
        GameFunctionLibrary.PlayRandomAudioAtPosition(damageData.audioClips, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Seems like this doesnt always trigger when enabling/disabling the hitbox.
        IDamageable damageable = collision.GetComponentInChildren<IDamageable>();
        if (damageable == null)
        {
            Debug.Log("DamageHitBox::OnTriggerEnter2D: Could not find IDamagable in children of target.");
            return;
        }

        damageData.target = collision.gameObject;

        if (owningPlayerPawn && collision.GetComponent<PlayerPawn>())
        {
            Debug.Log("DamageHitBox::OnTriggerEnter2D: Both owner and target is player.");
            return;
        }
        if (owningEnemyPawn && collision.GetComponent<EnemyPawn>())
        {
            Debug.Log("DamageHitBox::OnTriggerEnter2D: Both owner and target is enemy.");
            return;
        }

        damageable.TakeDamage(damageData);
    }

    // Code fix to use instead of on triger enter:

    /*private void test()
    {
        // Adjust the box center relative to the object's position if needed
        Vector2 adjustedBoxCenter = (Vector2)transform.position + boxCenter;

        // Get all colliders within the box area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(adjustedBoxCenter, boxSize, boxAngle, layerMask);

        // Process the colliders
        foreach (Collider2D collider in colliders)
        {
            // Perform your logic here with each collider
            Debug.Log("Picked object: " + collider.gameObject.name);
        }
    }*/
}
