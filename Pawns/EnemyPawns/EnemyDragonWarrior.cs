using UnityEngine;

public class EnemyDragonWarrior : EnemyPawn
{
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_MeleeAttackState slamStateData;

    [SerializeField] private DragonWarrior_FlyStateData flyStateData;

    [SerializeField] private GameObject slamDamageHitBox;

    public EnemyDragonWarrior_ChargeState ChargeState { get; private set; }
    public EnemyDragonWarrior_FlyState FlyState { get; private set; }
    public EnemyDragonWarrior_IdleState IdleState { get; private set; }
    public EnemyDragonWarrior_LookForPlayerState LookForPlayerState { get; private set; }
    public EnemyDragonWarrior_MoveState MoveState { get; private set; }
    public EnemyDragonWarrior_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyDragonWarrior_SlamState SlamState { get; private set; }

    protected override void Start()
    {
        base.Start();
        ChargeState = new EnemyDragonWarrior_ChargeState(this, StateMachine, AnimationConstants.ANIM_PARAM_CHARGE, chargeStateData);
        FlyState = new EnemyDragonWarrior_FlyState(this, StateMachine, Animator.StringToHash(flyStateData.animationName), flyStateData);
        IdleState = new EnemyDragonWarrior_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData);
        LookForPlayerState = new EnemyDragonWarrior_LookForPlayerState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, lookForPlayerStateData);
        MoveState = new EnemyDragonWarrior_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData);
        PlayerDetectedState = new EnemyDragonWarrior_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData);
        SlamState = new EnemyDragonWarrior_SlamState(this, StateMachine, AnimationConstants.ANIM_PARAM_SLAM, slamDamageHitBox, slamStateData);

        StateMachine.Initialize(IdleState);
    }

    public override bool CheckPlayerInCloseRangeAction()
    {
        return base.CheckPlayerInCloseRangeAction() && SlamState.IsMeleeAttackReady();
    }

    public override bool CheckPlayerInLongRangeAction()
    {
        return base.CheckPlayerInLongRangeAction() && (ChargeState.IsChargeReady() || FlyState.IsFlyReady());
    }

    public override void TakeDamage(Types.DamageData damageData)
    {
        base.TakeDamage(damageData);
        if (IsAlive() && (StateMachine.currentState == IdleState || StateMachine.currentState == MoveState))
            StateMachine.ChangeState(LookForPlayerState);
    }

    private void TriggerStartAscend() => FlyState.StartAscend();
    private void TriggerStartDescend() => FlyState.StartDescend();
    private void TriggerFinishedLandingAnimation() => FlyState.OnFinishLandingAnimation();
}
