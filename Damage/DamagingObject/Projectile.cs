using UnityEngine;

public class Projectile : DamagingObject
{
    [SerializeField] protected float speed;
    [SerializeField] protected Vector2 currentDir;
    [SerializeField] protected bool destroyedByDestructibles = false;
    [SerializeField] protected bool destroyedByGround = false;
    [SerializeField] protected GameObject sfxChildGameObject;

    protected Rigidbody2D rb;
    protected Vector2 currentSpeed;

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {
        if (rb != null && speed != 0f)
        {
            currentSpeed = rb.velocity;
        }
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
    protected override void ProcessCollision(GameObject other)
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

        base.ProcessCollision(other);
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
        rb.velocity = new Vector2(-rb.velocity.x * 1.2f, -rb.velocity.y * 1.2f);
    #endregion
}
