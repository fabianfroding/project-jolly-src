using UnityEngine;

public class EnemyDruid : EnemyPawn
{
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;

    public EnemyDruid_ChargeState ChargeState { get; private set; }
    public EnemyDruid_IdleState IdleState { get; private set; }
    public EnemyDruid_LookForPlayerState LookForPlayerState { get; private set; }
    public EnemyDruid_MoveState MoveState { get; private set; }
    public EnemyDruid_PlayerDetectedState PlayerDetectedState { get; private set; }

    protected override void Start()
    {
        base.Start();
        ChargeState = new EnemyDruid_ChargeState(this, StateMachine, AnimationConstants.ANIM_PARAM_CHARGE, chargeStateData, this);
        IdleState = new EnemyDruid_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        LookForPlayerState = new EnemyDruid_LookForPlayerState(this, StateMachine, AnimationConstants.ANIM_PARAM_LOOK_FOR_PLAYER, lookForPlayerStateData, this);
        MoveState = new EnemyDruid_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        PlayerDetectedState = new EnemyDruid_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        StateMachine.Initialize(MoveState);
    }
}
