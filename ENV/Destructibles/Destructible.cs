using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] protected int health = 1;
    [SerializeField] private GameObject destroyedSFXPrefab; // The particle system for when the destructible is destroyed.
    [SerializeField] private string destroyedAnimName;
    [SerializeField] private AudioSource onHitSound;
    [SerializeField] private AudioSource onDeathSound;

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
            health--;
            if (health == 0)
                DestroySelf();
        }
    }

    public int GetHealth()
    {
        return health;
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
}
