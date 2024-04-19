using System;
using System.Collections;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    [SerializeField] protected Types.DamageData damageData;

    [Tooltip("Determines how long before the object should die if not hitting anything.")]
    [SerializeField] protected float lifetime = 0.15f;

    [SerializeField] protected GameObject birthSoundPrefab;
    [SerializeField] protected GameObject deathSoundPrefab;
    [SerializeField] protected GameObject deathSFXPrefab;
    [SerializeField] protected bool destroyOnImpact = true;

    public GameObject Source { get; protected set; }
    protected SpriteRenderer spriteRenderer;

    public static event Action OnHit;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (birthSoundPrefab != null)
        {
            GameObject GO = Instantiate(birthSoundPrefab);
            GO.transform.SetParent(null);
            GO.transform.position = transform.position;
        }

        StopCoroutine(DestroySelf());
        StartCoroutine(DestroySelf(lifetime));
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        ProcessCollision(other.gameObject);
    }
    #endregion

    #region Other Functions
    public Types.DamageData GetDamageData() => damageData;
    public float GetLifetime() => lifetime;
    public void SetSource(GameObject source) => Source = source;
    public void SetDamageData(Types.DamageData newDamageData) => damageData = newDamageData;
    public bool IsProjectile() => GetType() == typeof(Projectile);

    protected virtual void ProcessCollision(GameObject other)
    {
        if (!other.GetComponent<Collider2D>().isTrigger && Source != other)
        {
            // Prevent player damage-objects from colliding with NPCs.
            if (Source != null && Source.GetComponent<PlayerPawn>() && other.GetComponent<NPCPawn>())
                return;

            PawnBase pawn = other.GetComponent<PawnBase>();
            if (pawn != null)
            {
                damageData.source = Source;
                damageData.target = other;
                //pawn.TakeDamage(damageData);
            }

            IDamageable damageable = other.GetComponentInChildren<IDamageable>();
            if (damageable != null)
            {
                damageData.source = Source;
                damageData.target = other;
                damageable.TakeDamage(damageData);
            }

            if (destroyOnImpact)
            {
                StopCoroutine(DestroySelf(0f));
                StartCoroutine(DestroySelf(0f));
            }

            OnHit?.Invoke();
        }

        // Duct tape solution.
        if (Source != other)
            InstantiateDeathSFX(other);
    }

    protected IEnumerator DestroySelf(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        if (deathSoundPrefab != null)
        {
            GameObject hitSound = Instantiate(deathSoundPrefab);
            hitSound.transform.parent = null;
            hitSound.transform.position = transform.position;
        }
        Destroy(gameObject);
    }

    protected void InstantiateDeathSFX(GameObject at)
    {
        if (deathSFXPrefab != null)
        {
            GameObject hitSFX = Instantiate(deathSFXPrefab, transform);
            hitSFX.transform.parent = null;
            hitSFX.transform.position = at.transform.position;
        }
    }
    #endregion
}
