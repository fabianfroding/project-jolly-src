using System.Collections;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private AudioClip playerCatchSound;
    [SerializeField] private float returnAfterDuration = 1.75f;

    private bool isReturning = false;

    protected override void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer("PlayerProjectile"));
        base.Awake();
    }

    protected void OnEnable()
    {
        StartCoroutine(ReturnAfterDelay());
    }

    private void Return()
    {
        if (isReturning)
            return;
        isReturning = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-rb.linearVelocity.x, -rb.linearVelocity.y);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer("PlayerProjectile"), false);

        destroyOnImpact = true;
        destroyedByDestructibles = true;
        destroyedByGround = true;
    }

    private IEnumerator ReturnAfterDelay()
    {
        yield return new WaitForSeconds(returnAfterDuration);
        Return();
    }

    protected override void ProcessCollision(GameObject other)
    {
        if (isReturning)
        {
            if (other.GetComponent<PlayerCharacter>())
            {
                GameFunctionLibrary.PlayAudioAtPosition(playerCatchSound, transform.position);
                // TODO: Give player back the boomerang.
            }
            Destroy(gameObject);
        }
        else
        {
            Return();

            IDamageable damageable = other.GetComponentInChildren<IDamageable>();
            if (damageable != null)
            {
                damageData.source = Source;
                damageData.target = other;
                damageable.TakeDamage(damageData);
                damageData.source.GetComponent<CharacterBase>().BroadcastOnDealtDamage();
            }
        }
    }
}
