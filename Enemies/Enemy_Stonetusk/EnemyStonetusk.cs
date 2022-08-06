using UnityEngine;

public class EnemyStonetusk : Enemy
{
    public DeadState DeadState { get; private set; }
    public EnemyStonetusk_IdleState IdleState { get; private set; }
    public EnemyStonetusk_MoveState MoveState { get; private set; }
    public EnemyStonetusk_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyStonetusk_LookForPlayerState LookForPlayerState { get; private set; }
    public EnemyStonetusk_ChargeState ChargeState { get; private set; }

    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    public D_ChargeState chargeStateData;

    protected override void Start()
    {
        base.Start();

        DeadState = new DeadState(this, StateMachine, AnimationConstants.ANIM_PARAM_DEAD, deadStateData);
        IdleState = new EnemyStonetusk_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        MoveState = new EnemyStonetusk_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        PlayerDetectedState = new EnemyStonetusk_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        LookForPlayerState = new EnemyStonetusk_LookForPlayerState(this, StateMachine, AnimationConstants.ANIM_PARAM_LOOK_FOR_PLAYER, lookForPlayerStateData, this);
        ChargeState = new EnemyStonetusk_ChargeState(this, StateMachine, AnimationConstants.ANIM_PARAM_CHARGE, chargeStateData, this);

        StateMachine.Initialize(IdleState);
    }

    protected override void Death()
    {
        base.Death();
        StateMachine.ChangeState(DeadState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }



}
