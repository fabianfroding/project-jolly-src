using UnityEngine;

public class EnemyGundyr : EnemyPawn
{
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackSecondStateData;
    [SerializeField] private D_MeleeAttackState slamStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;

    [SerializeField] private GameObject meleeAttackSecondDamageHitBox;
    [SerializeField] private GameObject slamDamageHitBox;

    public EnemyGundyr_IdleState IdleState { get; private set; }
    public EnemyGundyr_MeleeAttackState MeleeAttackState { get; private set; }
    public EnemyGundyr_MeleeAttackSecondState MeleeAttackSecondState { get; private set; }
    public EnemyGundyr_MoveState MoveState { get; private set; }
    public EnemyGundyr_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyGundyr_SlamState SlamState { get; private set; }

    protected override void Start()
    {
        base.Start();
        IdleState = new EnemyGundyr_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        MeleeAttackState = new EnemyGundyr_MeleeAttackState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK, meleeAttackDamageHitBox, meleeAttackStateData, this);
        MeleeAttackSecondState = new EnemyGundyr_MeleeAttackSecondState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK_SECOND, meleeAttackSecondDamageHitBox, meleeAttackSecondStateData, this);
        MoveState = new EnemyGundyr_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        PlayerDetectedState = new EnemyGundyr_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        SlamState = new EnemyGundyr_SlamState(this, StateMachine, AnimationConstants.ANIM_PARAM_SLAM, slamDamageHitBox, slamStateData, this);

        StateMachine.Initialize(IdleState);
    }
}
