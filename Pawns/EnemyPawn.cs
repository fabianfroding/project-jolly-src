using UnityEngine;

public class EnemyPawn : PawnBase, IParriable
{
    public D_Enemy enemyData;

    public AnimationToStateMachine ATSM { get; protected set; }
    public Collider2D EnemyCollider { get; protected set; }
    public FiniteStateMachine StateMachine { get; protected set; }

    public Vector3 InitialPosition { get; protected set; }
    public bool InitialFacing { get; protected set; }

    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected GameObject meleeAttackDamageHitBox;

    public AIVisionComponent AIVision => aiVision ? aiVision : Core.GetCoreComponent(ref aiVision);
    protected AIVisionComponent aiVision;
    protected CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : Core.GetCoreComponent(ref collisionSenses);
    protected CollisionSenses collisionSenses;

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();
        ATSM = GetComponent<AnimationToStateMachine>();
        EnemyCollider = GetComponent<Collider2D>();
        StateMachine = new FiniteStateMachine();

        // Prevent enemy from colliding with NPCs.
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY), LayerMask.NameToLayer(EditorConstants.LAYER_NPC));
    }

    protected override void Start()
    {
        base.Start();

        HealthComponent.SetHealth(HealthComponent.GetMaxHealth());
        Combat.SetCurrentStunResistance(Combat.GetStunResistance());

        InitialPosition = transform.position;
        InitialFacing = Core.GetCoreComponent<Movement>().FacingDirection == 1;

        CheckRespawn();
    }

    protected override void Update()
    {
        base.Update();

        // Everytime Update is called on the Enemy, we call the LogicUpdate on the state.
        StateMachine.currentState?.LogicUpdate();
        Combat.CheckStunRecoveryTime();
    }

    protected virtual void FixedUpdate()
    {
        StateMachine.currentState?.PhysicsUpdate();
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

        if (AIVision && AIVision.TargetPlayerPawn)
        {
            return Vector3.Distance(playerCheck.position, AIVision.TargetPlayerPawn.transform.position) < enemyData.minAggroDistance;
        }

        return false;
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        if (CheckGroundInRange(playerCheck.position, transform.right, enemyData.maxAggroDistance))
        {
            return false;
        }

        if (AIVision && AIVision.TargetPlayerPawn)
        {
            return Vector3.Distance(playerCheck.position, AIVision.TargetPlayerPawn.transform.position) < enemyData.maxAggroDistance;
        }

        return false;
    }

    protected bool CheckGroundInRange(Vector3 originPos, Vector3 direction, float range)
    {
        return Physics2D.Raycast(originPos, direction, range, enemyData.groundLayer);
    }

    public override void Death()
    {
        AddToKilledEnemies();

        base.Death();
        gameObject.SetActive(false);
    }

    protected virtual bool CanBeParried(Types.DamageData damageData) 
    {
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
}
