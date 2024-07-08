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

        HealthComponent.SetHealth(HealthComponent.GetMaxHealth().Value);
        Combat.SetCurrentStunResistance(Combat.GetStunResistance());

        InitialPosition = transform.position;
        InitialFacing = Core.GetCoreComponent<Movement>().FacingDirection == 1;
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

    private Vector2 GetDirectionToPlayerTarget()
    {
        Vector2 directionToPlayer = aiVision.TargetPlayerPawn.transform.position - playerCheck.position;
        directionToPlayer.Normalize();
        return directionToPlayer;
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        if (!AIVision.TargetPlayerPawn)
            return false;
        if (!AIVision.TargetPlayerPawn.IsAlive() || AIVision.IsPlayerBehind())
            return false;
        if (CheckGroundInRange(playerCheck.position, transform.right, enemyData.closeRangeActionDistance))
            return false;

#if UNITY_EDITOR
        Debug.DrawLine(playerCheck.position, playerCheck.position + ((Vector3)GetDirectionToPlayerTarget() * enemyData.longRangeActionDistance), Color.red);
#endif

        return Physics2D.Raycast(playerCheck.position, GetDirectionToPlayerTarget(), enemyData.closeRangeActionDistance, enemyData.playerLayer);
    }

    public virtual bool CheckPlayerInLongRangeAction()
    {
        if (!AIVision.TargetPlayerPawn)
            return false;
        if (!AIVision.TargetPlayerPawn.IsAlive() || AIVision.IsPlayerBehind())
            return false;
        if (CheckGroundInRange(playerCheck.position, transform.right, enemyData.longRangeActionDistance))
            return false;

#if UNITY_EDITOR
        Debug.DrawLine(playerCheck.position, playerCheck.position + ((Vector3)GetDirectionToPlayerTarget() * enemyData.longRangeActionDistance), Color.yellow);
#endif

        return Physics2D.Raycast(playerCheck.position, GetDirectionToPlayerTarget(), enemyData.longRangeActionDistance, enemyData.playerLayer);
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        if (playerCheck == null || CheckGroundInRange(playerCheck.position, transform.right, enemyData.minAggroDistance))
            return false;
        if (AIVision && AIVision.TargetPlayerPawn && AIVision.TargetPlayerPawn.IsAlive())
            return Vector3.Distance(playerCheck.position, AIVision.TargetPlayerPawn.transform.position) < enemyData.minAggroDistance;
        return false;
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        if (CheckGroundInRange(playerCheck.position, transform.right, enemyData.maxAggroDistance))
            return false;
        if (AIVision && AIVision.TargetPlayerPawn && AIVision.TargetPlayerPawn.IsAlive())
            return Vector3.Distance(playerCheck.position, AIVision.TargetPlayerPawn.transform.position) < enemyData.maxAggroDistance;
        return false;
    }

    protected bool CheckGroundInRange(Vector3 originPos, Vector3 direction, float range)
    {
        return Physics2D.Raycast(originPos, direction, range, enemyData.groundLayer);
    }

    public override void TakeDamage(Types.DamageData damageData)
    {
        base.TakeDamage(damageData);
        if (HealthComponent.IsAlive() && damageData.isKillZone)
            HealthComponent.Kill();
    }

    public override void Death()
    {
        base.Death();
        gameObject.SetActive(false);
    }

    protected virtual bool CanBeParried(Types.DamageData damageData) 
    {
        return Combat.CanBeParried();
    }

    public virtual void Parried() {} // Interface implementation.

    public bool IsPlayerBehind()
    {
        if (AIVision)
            return false;
        return AIVision.IsPlayerBehind();
    }

    public bool ShouldFlipIfTargetIsBehind()
    {
        if (!AIVision) return false;
        return AIVision.ShouldFlipIfTargetIsBehind();
    }
}
