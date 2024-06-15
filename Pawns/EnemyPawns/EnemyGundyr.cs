using UnityEngine;

public class EnemyGundyr : EnemyPawn
{
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;

    public EnemyGundyr_IdleState IdleState { get; private set; }
    public EnemyGundyr_MeleeAttackState MeleeAttackState { get; private set; }
    public EnemyGundyr_MoveState MoveState { get; private set; }
    public EnemyGundyr_PlayerDetectedState PlayerDetectedState { get; private set; }

    protected override void Start()
    {
        base.Start();
        IdleState = new EnemyGundyr_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        MeleeAttackState = new EnemyGundyr_MeleeAttackState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK, meleeAttackDamageHitBox, meleeAttackStateData, this);
        MoveState = new EnemyGundyr_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        PlayerDetectedState = new EnemyGundyr_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        
        StateMachine.Initialize(IdleState);
    }
}
