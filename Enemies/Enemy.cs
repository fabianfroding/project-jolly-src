using System.Collections;
using UnityEngine;

public class Enemy : Entity, IParriable
{
    public D_Enemy enemyData;
    public FiniteStateMachine StateMachine { get; protected set; }
    public AnimationToStateMachine ATSM { get; protected set; }
    public Vector3 InitialPosition { get; protected set; }
    public bool InitialFacing { get; protected set; }
    public int LastDamageDirection { get; protected set; }
    public GameObject Target { get; protected set; }
    public FieldOfView FoV { get; protected set; }

    [SerializeField] protected Transform playerCheck;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? Core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();
        ATSM = GetComponent<AnimationToStateMachine>();
        FoV = GetComponent<FieldOfView>();
        StateMachine = new FiniteStateMachine();

        // Prevent enemy from colliding with NPCs.
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY), LayerMask.NameToLayer(EditorConstants.LAYER_NPC));
    }

    protected override void Start()
    {
        base.Start();

        Stats.IncreaseHealth(Stats.GetMaxHealth());
        Combat.SetCurrentStunResistance(Stats.GetStunResistance());

        InitialPosition = transform.position;
        InitialFacing = Core.GetCoreComponent<Movement>().FacingDirection == 1;

        CheckRespawn();
    }

    protected override void Update()
    {
        base.Update();

        // Everytime Update is called on the Enemy, we call the LogicUpdate on the state.
        if (StateMachine.currentState != null)
        {
            StateMachine.currentState.LogicUpdate();
        }

        CheckFOVTarget();
        Combat.CheckStunRecoveryTime();
    }

    protected virtual void FixedUpdate()
    {
        if (StateMachine.currentState != null)
        {
            StateMachine.currentState.PhysicsUpdate();
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (Core != null)
        {
            Gizmos.DrawLine(CollisionSenses.WallCheck.position, CollisionSenses.WallCheck.position + (Vector3)(enemyData.wallCheckDistance * Movement.FacingDirection * Vector2.right));
            Gizmos.DrawLine(CollisionSenses.LedgeCheck.position, CollisionSenses.LedgeCheck.position + (Vector3)(enemyData.wallCheckDistance * Movement.FacingDirection * Vector2.down));

            if (playerCheck != null)
            {
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(enemyData.closeRangeActionDistance * Movement.FacingDirection * Vector2.right), 0.2f);
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(enemyData.minAggroDistance * Movement.FacingDirection * Vector2.right), 0.2f);
                Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(enemyData.maxAggroDistance * Movement.FacingDirection * Vector2.right), 0.2f);
            }
        }
    }
    #endregion

    #region Check Functions
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        if (CheckGroundInRange(playerCheck.position, transform.right, enemyData.closeRangeActionDistance))
        {
            return false;
        }
        return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.closeRangeActionDistance, enemyData.playerLayer);
    }

    public virtual bool CheckPlayerInLongRangeAction()
    {
        if (CheckGroundInRange(playerCheck.position, transform.right, enemyData.longRangeActionDistance))
        {
            return false;
        }
        //Debug.DrawLine(playerCheck.position, playerCheck.position + (transform.right * enemyData.longRangeActionDistance), Color.cyan);
        return Physics2D.Raycast(playerCheck.position, transform.right, enemyData.longRangeActionDistance, enemyData.playerLayer);
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        if (playerCheck == null || CheckGroundInRange(playerCheck.position, transform.right, enemyData.minAggroDistance))
        {
            return false;
        }

        if (FoV != null && FoV.Target != null)
        {
            return Vector3.Distance(playerCheck.position, FoV.Target.transform.position) < enemyData.minAggroDistance;
        }

        return false;
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        if (CheckGroundInRange(playerCheck.position, transform.right, enemyData.maxAggroDistance))
        {
            return false;
        }

        if (FoV != null && FoV.Target != null)
        {
            return Vector3.Distance(playerCheck.position, FoV.Target.transform.position) < enemyData.maxAggroDistance;
        }

        return false;
    }

    protected bool CheckGroundInRange(Vector3 originPos, Vector3 direction, float range)
    {
        return Physics2D.Raycast(originPos, direction, range, enemyData.groundLayer);
    }

    protected void CheckFOVTarget()
    {
        if (FoV != null)
        {
            if (enemyData.targetLockDuration > 0.0f && Target == null)
            {
                Target = FoV.Target;
                if (Target != null)
                {
                    StopCoroutine(ResetTargetDelayed());
                    StartCoroutine(ResetTargetDelayed());
                }
            }
            else if (enemyData.targetLockDuration == 0.0f)
            {
                Target = FoV.Target;
            }
        }
    }
    #endregion

    #region Other Functions
    public override void TakeDamage(GameObject source, Damage damage, GameObject damagingObject = null)
    {
        if (CanBeParried(source, damagingObject)) return;

        base.TakeDamage(source, damage, damagingObject);

        // Check if attack came from the right (-1) or left (1).
        LastDamageDirection = damage.direction.x > transform.position.x ? -1 : 1;

        Combat.ApplyStunDamage(damage.stunDamageAmount);
        //DamageHop(enemyData.damageHopSpeed); // Enable if we want enemies to "hop" slightly when damaged.
    }

    protected override void Death()
    {
        AddToKilledEnemies();
        EnemyListRepository.AddEnemyListNumKilled(gameObject.name);

        base.Death();
        gameObject.SetActive(false);
    }

    public void ResetTarget() => Target = null;
    protected virtual IEnumerator ResetTargetDelayed()
    {
        yield return new WaitForSeconds(enemyData.targetLockDuration);
        Target = null;
    }

    protected virtual bool CanBeParried(GameObject source, GameObject damagingObject) 
    {
        Entity entity = source.GetComponent<Entity>();
        if (entity != null)
        {
            DamagingObject dmgObject = damagingObject.GetComponent<DamagingObject>();
            if (dmgObject != null && !dmgObject.IsProjectile())
            {
                if (entity.Core.GetCoreComponent<Movement>().FacingDirection != Movement.FacingDirection)
                    return Combat.CanBeParried();
            }
            return false;
        }
        return Combat.CanBeParried();
    }

    public virtual void Parried() {} // Interface implementation.

    protected void AddToKilledEnemies()
    {
        if (enemyData.enemyRespawnType == D_Enemy.EnemyRespawnType.NEVER)
        {
            EnemyRepository.AddToKilledEnemies(gameObject.name, true);
        }
        else if (enemyData.enemyRespawnType == D_Enemy.EnemyRespawnType.ON_SAVE)
        {
            EnemyRepository.AddToKilledEnemies(gameObject.name, false);
        }
    }

    protected void CheckRespawn()
    {
        if (EnemyRepository.HasBeenKilled(gameObject.name))
        {
            if (enemyData.enemyRespawnType == D_Enemy.EnemyRespawnType.NEVER ||
                enemyData.enemyRespawnType == D_Enemy.EnemyRespawnType.ON_SAVE)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void TriggerBehaviour(GameObject triggeringObject) { }
    #endregion
}
