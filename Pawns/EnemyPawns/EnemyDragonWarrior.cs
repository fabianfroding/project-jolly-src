using UnityEngine;

[System.Serializable]
public struct DragonWarrior_FlyStateData
{
    public float flyStartDelay;
    public float flyStartDescendDelay;
    public float flyCooldown;
    public AudioClip flyImpactAudioClip;
    public AudioClip flyStartSound;
    public AudioClip flyStartDescendAudioClip;
    public string animationName;
}

public class EnemyDragonWarrior : EnemyPawn
{
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_MeleeAttackState slamStateData;

    [SerializeField] private DragonWarrior_FlyStateData flyStateData;

    [SerializeField] private GameObject slamDamageHitBox;

    public EnemyDragonWarrior_ChargeState ChargeState { get; private set; }
    public EnemyDragonWarrior_FlyState FlyState { get; private set; }
    public EnemyDragonWarrior_IdleState IdleState { get; private set; }
    public EnemyDragonWarrior_MoveState MoveState { get; private set; }
    public EnemyDragonWarrior_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyDragonWarrior_SlamState SlamState { get; private set; }

    protected override void Start()
    {
        base.Start();
        ChargeState = new EnemyDragonWarrior_ChargeState(this, StateMachine, AnimationConstants.ANIM_PARAM_CHARGE, chargeStateData);
        FlyState = new EnemyDragonWarrior_FlyState(this, StateMachine, Animator.StringToHash(flyStateData.animationName), flyStateData);
        IdleState = new EnemyDragonWarrior_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData);
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

    private void TriggerStartAscend() => FlyState.StartAscend();
    private void TriggerStartDescend() => FlyState.StartDescend();
}
