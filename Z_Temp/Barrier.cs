using UnityEngine;

/* This script should be attached to the Barrier game object prefab.
 * It should only be accessed through the PlayerBarrier script.
 */

public class Barrier : MonoBehaviour
{
    [SerializeField] private string animNameBarrierIdle = "Barrier_Idle";
    [SerializeField] private string animNameBarrierBreak = "Barrier_Break";
    [SerializeField] private AudioSource barrierEndSoundRef;
    [SerializeField] private float duration = 2f;
    private Animator animator;
    private bool isActive = true;

    //==================== PUBLIC ====================//
    public void DestroySelf()
    {
        // Cancel the invoke from Start() to prevent endSound from playing at duration
        // end if it already ended from breaking with reflect ability equipped.
        CancelInvoke("DestroySelf");

        isActive = false;
        barrierEndSoundRef.Play();
        animator.Play(animNameBarrierBreak);
        Invoke("HideSpriteRenderer", 0.05f);
        Destroy(gameObject, barrierEndSoundRef.clip.length);
    }

    public bool IsActive()
    {
        return isActive;
    }

    //==================== PRIVATE ====================//
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play(animNameBarrierIdle);
    }

    private void Start()
    {
        isActive = true;
        Invoke("DestroySelf", duration);
    }

    private bool IsColliderHostileProjectile(Collider2D other)
    {
        return other.gameObject.CompareTag(EditorConstants.TAG_PROJECTILE) &&
            other.GetComponent<Projectile>().Source.CompareTag(EditorConstants.TAG_ENEMY);
    }

    private void HideSpriteRenderer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool reflect = false;
        /*bool barrierReflectEquipped = UIManager.GetUIManagerScript().GetEquipmentMenu().IsEquipped("BarrierReflect");
        if (barrierReflectEquipped)
        {
            GameObject source = null;
            if (IsColliderHostileProjectile(other))
            {
                source = other.gameObject.GetComponent<Projectile>().Source;
                other.gameObject.GetComponent<Projectile>().SetSource(transform.parent.gameObject);
                other.gameObject.GetComponent<Projectile>().InvertDirection();
                reflect = true;
            }
            else if (other.gameObject.CompareTag(EditorConstants.TAG_ENEMY)) source = other.gameObject;
            if (source != null)
            {
                DestroySelf(); // Cooldown still activates in Invoke in Player, but it's not a big issue.
                if (!reflect)
                {
                    // Create new reflected damage and copy all fields from incoming damage except for damage amount.
                    //Damage incomingDmg = other.GetComponent<Projectile>().GetDamage();
                    //Damage reflectedDmg = new Damage(1, incomingDmg.isAoE, incomingDmg.isProjectile, incomingDmg.envDamage, incomingDmg.damageType);
                    //source.GetComponent<Unit>().TakeDamage(gameObject.transform.parent.gameObject, reflectedDmg);
                } 
            }
        }*/

        if (IsColliderHostileProjectile(other) && !reflect)
        {
            // TODO: Find a way to play projectile death sound before destroying it (add method CreateDeathSound).
            // Or just call Destroy with time equal to length of audio clip... + Should prob be done in proj class, not here.
            Destroy(other.gameObject);
        }
    }
}
