using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private AudioClip playerCatchSound;

    private bool isReturning = false;

    protected override void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer("PlayerProjectile"));
        base.Awake();
    }

    protected override void ProcessCollision(GameObject other)
    {
        if (isReturning)
        {
            if (other.GetComponent<PlayerPawn>())
            {
                GameFunctionLibrary.PlayAudioAtPosition(playerCatchSound, transform.position);
                // TODO: Give player back the boomerang.
            }
            Destroy(gameObject);
        }
        else
        {
            isReturning = true;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-rb.velocity.x, -rb.velocity.y);

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer("PlayerProjectile"), false);

            destroyOnImpact = true;
            destroyedByDestructibles = true;
            destroyedByGround = true;

            IDamageable damageable = other.GetComponentInChildren<IDamageable>();
            if (damageable != null)
            {
                damageData.source = Source;
                damageData.target = other;
                damageable.TakeDamage(damageData);
                damageData.source.GetComponent<PawnBase>().BroadcastOnDealtDamage();
            }
        }
    }
}
