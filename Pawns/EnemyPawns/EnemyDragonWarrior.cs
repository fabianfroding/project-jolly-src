using UnityEngine;

public class EnemyDragonWarrior : EnemyPawn
{
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_MeleeAttackState slamStateData;

    [SerializeField] private GameObject slamDamageHitBox;

    public EnemyDragonWarrior_ChargeState ChargeState { get; private set; }
    public EnemyDragonWarrior_IdleState IdleState { get; private set; }
    public EnemyDragonWarrior_MoveState MoveState { get; private set; }
    public EnemyDragonWarrior_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyDragonWarrior_SlamState SlamState { get; private set; }

    protected override void Start()
    {
        base.Start();
        ChargeState = new EnemyDragonWarrior_ChargeState(this, StateMachine, AnimationConstants.ANIM_PARAM_CHARGE, chargeStateData, this);
        IdleState = new EnemyDragonWarrior_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        MoveState = new EnemyDragonWarrior_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        PlayerDetectedState = new EnemyDragonWarrior_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        SlamState = new EnemyDragonWarrior_SlamState(this, StateMachine, AnimationConstants.ANIM_PARAM_SLAM, slamDamageHitBox, slamStateData, this);

        StateMachine.Initialize(IdleState);
    }
}
