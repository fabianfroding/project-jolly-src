using System.Collections;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    [SerializeField] protected Damage damage;

    [Tooltip("Determines how long before the object should die if not hitting anything.")]
    [SerializeField] protected float lifetime = 0.15f;

    [SerializeField] protected GameObject birthSoundPrefab;
    [SerializeField] protected GameObject deathSoundPrefab;
    [SerializeField] protected GameObject deathSFXPrefab;

    public GameObject Source { get; protected set; }
    protected SpriteRenderer spriteRenderer;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (birthSoundPrefab != null)
        {
            Instantiate(birthSoundPrefab);
            birthSoundPrefab.transform.SetParent(null);
            birthSoundPrefab.transform.position = transform.position;
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
    public Damage GetDamage() => damage;
    public float GetLifetime() => lifetime;
    public void SetSource(GameObject source) => Source = source;
    public bool IsProjectile() => GetType() == typeof(Projectile);

    protected virtual void ProcessCollision(GameObject other)
    {
        if (!other.GetComponent<Collider2D>().isTrigger && Source != other)
        {
            // Prevent player damage-objects from colliding with NPCs.
            if (Source != null && Source.CompareTag(EditorConstants.TAG_PLAYER) && other.CompareTag(EditorConstants.TAG_NPC))
            {
                return;
            }

            Entity entity = other.GetComponent<Entity>();
            if (entity != null)
            {
                entity.TakeDamage(Source, damage, gameObject);
            }

            StopCoroutine(DestroySelf(0f));
            StartCoroutine(DestroySelf(0f));
        }

        // Duct tape solution.
        if (Source != other)
        {
            InstantiateDeathSFX(other);
        }
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
