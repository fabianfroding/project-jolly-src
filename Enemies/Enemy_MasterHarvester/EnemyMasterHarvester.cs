using System.Collections;
using UnityEngine;

public class EnemyMasterHarvester : Enemy
{
    [SerializeField] private BoxCollider2D tpArea;
    [SerializeField] private GameObject teleportIndicatorPrefab;
    [SerializeField] private GameObject laughSoundPrefab;
    [SerializeField] private GameObject hitSoundExtraPrefab;

    [SerializeField] private D_IdleState idleStateData;

    public EnemyMasterHarvester_IdleState IdleState { get; private set; }

    private HarvesterDodge dodge;
    private AbilityDarkSpheres darkSpheres;
    private GameObject teleportIndicatorInstance;

    protected override void Start()
    {
        base.Start();
        IdleState = new EnemyMasterHarvester_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData);
    }

    //==================== PUBLIC ====================//
    public override void TriggerBehaviour(GameObject triggeringObject)
    {
        //SetTarget(triggeringObject);
    }

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

    public AbilityDarkSpheres GetDarkSpheresAbility()
    {
        return darkSpheres;
    }

    protected override void Awake()
    {
        base.Awake();
        dodge = GetComponent<HarvesterDodge>();
        darkSpheres = GetComponent<AbilityDarkSpheres>();
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

    private IEnumerator Tick()
    {
        if (Target != null && !darkSpheres.IsActive())
        {
            if (true /*target.GetComponent<Unit>().GetHealth() > 0*/)
            {
                if (teleportIndicatorInstance == null)
                {
                    // Use abilities.
                    int random = Random.Range(1, 6);
                    // 1-2 TP
                    // 3-4 Orb
                    // 5-6 Dodge
                    // 7-8 Glide
                    // 9 Fake Dodge
                    // 10 Multi Orbs

                    if (random <= 2)
                    {
                        Teleport();
                    }
                    else if (random <= 4)
                    {
                        //Attack();
                    }
                    else if (random <= 5)
                    {
                        if (darkSpheres.Initialize(gameObject))
                        {
                            //damageCount = 0;
                        }
                    }
                }
            }
            else
            {
                // Target is dead. Reset target var.
                //target = null;
            }
        }
        yield return new WaitForSeconds(1f /*globalCD*/);
        StartCoroutine(Tick());
    }
}
