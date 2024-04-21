using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected Types.DamageData damageData;
    [SerializeField] protected float lifetime = 2.5f;

    [SerializeField] protected GameObject birthSoundPrefab;
    [SerializeField] protected GameObject deathSoundPrefab;
    [SerializeField] protected GameObject deathSFXPrefab;
    [SerializeField] protected bool destroyOnImpact = true;

    [SerializeField] protected float speed;
    [SerializeField] protected Vector2 currentDir;
    [SerializeField] protected bool destroyedByDestructibles = false;
    [SerializeField] protected bool destroyedByGround = false;
    [SerializeField] protected GameObject sfxChildGameObject;

    public GameObject Source { get; protected set; }
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected Vector2 currentSpeed;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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

    protected void Update()
    {
        if (rb != null && speed != 0f)
        {
            currentSpeed = rb.velocity;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        ProcessCollision(other.gameObject);
    }

    protected void OnDestroy()
    {
        if (sfxChildGameObject != null)
        {
            sfxChildGameObject.transform.parent = null;
            sfxChildGameObject.GetComponent<ProjectileSFX>().StopEmitting();
        }
    }
    #endregion

    #region Other Functions
    protected virtual void ProcessCollision(GameObject other)
    {
        if (destroyedByDestructibles && other.CompareTag(EditorConstants.TAG_ENVIRONMENT))
        {
            StopCoroutine(DestroySelf());
            StartCoroutine(DestroySelf());
        }

        else if (destroyedByGround && other.CompareTag(EditorConstants.TAG_GROUND))
        {
            StopCoroutine(DestroySelf());
            StartCoroutine(DestroySelf());
        }

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
        }

        // Duct tape solution.
        if (Source != other)
            InstantiateDeathSFX(other);
    }

    public Types.DamageData GetDamageData() => damageData;
    public float GetLifetime() => lifetime;
    public void SetSource(GameObject source) => Source = source;
    public void SetDamageData(Types.DamageData newDamageData) => damageData = newDamageData;
    public bool IsProjectile() => GetType() == typeof(Projectile);

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

    public void Init(GameObject source, Vector2 dir)
    {
        Source = source;
        rb.velocity = speed * dir;
    }

    public Vector2 GetDirectionFromPoint(Vector3 point) => (transform.position - point).normalized;

    public void SetCurrentDirection(Vector2 dir) => currentDir = dir.normalized;

    public void SetDirection(Transform from, Transform to)
    {
        rb.velocity = (to.position - from.position).normalized;
        rb.velocity = new Vector2(rb.velocity.x * speed, rb.velocity.y * speed);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        rb.velocity = currentDir * speed;
    }

    public void SetRotation(Vector3 rotation) => transform.Rotate(rotation);

    public void InvertDirection() => 
        rb.velocity = new Vector2(-rb.velocity.x, -rb.velocity.y);
    #endregion
}
