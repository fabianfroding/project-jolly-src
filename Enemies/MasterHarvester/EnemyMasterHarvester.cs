using System.Collections;
using UnityEngine;

public class EnemyMasterHarvester : EnemyCharacter
{
    [SerializeField] private BoxCollider2D tpArea;
    [SerializeField] private GameObject teleportIndicatorPrefab;
    [SerializeField] private GameObject laughSoundPrefab;
    [SerializeField] private GameObject hitSoundExtraPrefab;

    [SerializeField] private D_IdleState idleStateData;

    public EnemyMasterHarvester_IdleState IdleState { get; private set; }

    private GameObject teleportIndicatorInstance;

    protected override void Start()
    {
        base.Start();
        IdleState = new EnemyMasterHarvester_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData);
    }

    //==================== PUBLIC ====================//

    /*public override void TakeDamage(Types.DamageData damageData)
    {
        if (/*CanSeePlayer() &&
            /*!dmg.isAoE && 
            damageData.damageType != Types.DamageType.MAGICAL &&
            !darkSpheres.IsActive())
        {
            if (dodge.StartDodge(damageData.source))
            {
                GameObject laughSound = Instantiate(laughSoundPrefab, transform.position, Quaternion.identity);
                Destroy(laughSound, laughSound.GetComponent<AudioSource>().clip.length);
                damageCount = 0;
            }
        }
        else
        {
            if (Random.Range(0, 3) == 2)
            {
                GameObject hitSoundExtra = Instantiate(hitSoundExtraPrefab, transform.position, Quaternion.identity);
                Destroy(hitSoundExtra, hitSoundExtra.GetComponent<AudioSource>().clip.length);
            }
            base.TakeDamage(damageData);
            damageCount++;
            if (damageCount > 2)
            {
                dodge.ResetCD();
                damageCount = 0;
            } 
        }
    }*/

    protected override void Awake()
    {
        base.Awake();
    }

    /*protected override bool Attack()
    {
        if (base.Attack())
        {
            Animator.Play(animNameCast);
            GameObject orb = Instantiate(harvesterOrbRef);
            orb.GetComponent<HarvesterOrb>().source = gameObject;
            orb.GetComponent<SpriteRenderer>().flipX = Core.Movement.FacingDirection == 1;
            orb.transform.position = transform.position;
            orb.GetComponent<HarvesterOrb>().SetDirection(gameObject.transform, target.transform);
            return true;
        }
        return false;
    }*/

    private void Teleport()
    {
        if (tpArea != null)
        {
            Bounds bounds = tpArea.bounds;
            teleportIndicatorInstance = Instantiate(teleportIndicatorPrefab);
            teleportIndicatorInstance.transform.position = new Vector2(Random.Range(bounds.min.x, bounds.max.x), transform.position.y);
            StartCoroutine(TeleportEnd(1.2f));
        }
    }

    private IEnumerator TeleportEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = teleportIndicatorInstance.transform.position;
        Destroy(teleportIndicatorInstance);
    }
}
