using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable
{
    [SerializeField] protected int durability = 1;
    [SerializeField] private GameObject destroyedSFXPrefab; // The particle system for when the destructible is destroyed.
    [SerializeField] private string destroyedAnimName;
    [SerializeField] private AudioSource onHitSound;
    [SerializeField] private AudioSource onDeathSound;
    [SerializeField] private GameObject deathSFX;

    private Animator animator;
    private bool destroyed = false;

    //==================== PUBLIC ====================//
    public virtual void TakeDamage()
    {
        if (!destroyed)
        {
            if (onHitSound != null)
            {
                onHitSound.Play();
            }
            durability--;
            if (durability == 0)
                DestroySelf();
        }
    }

    public int GetDurability()
    {
        return durability;
    }

    //==================== PROTECTED ====================//
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void DestroySelf()
    {
        destroyed = true;
        if (destroyedSFXPrefab != null)
        {
            GameObject sfx = Instantiate(destroyedSFXPrefab);
            sfx.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
            sfx.GetComponent<ParticleSystemRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
            Destroy(sfx, sfx.GetComponent<ParticleSystem>().main.duration);
        }

        if (onDeathSound != null)
            onDeathSound.Play();

        if (destroyedAnimName != null)
            animator.Play(destroyedAnimName);

        
    }

    public void TakeDamage(Types.DamageData damageData)
    {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            child.transform.SetParent(null);
            Rigidbody2D rb2d = child.gameObject.AddComponent<Rigidbody2D>();
            rb2d.AddForce(new Vector2(Random.Range(-200f, 200f), Random.Range(150f, 300f)));
            rb2d.AddTorque(Random.Range(50f, 200f));
            rb2d.gravityScale = 3f;
            Destroy(child.gameObject, 2.5f);
        }

        if (deathSFX)
        {
            GameObject deathSFXInstance = GameObject.Instantiate(deathSFX);
            deathSFXInstance.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}
