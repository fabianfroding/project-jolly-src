using UnityEngine;

public class EnemyJungleBeast : EnemyPawn
{
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackSecondStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;

    [SerializeField] private JungleBeast_JumpStateData jumpStateData;

    [SerializeField] private GameObject meleeAttackSecondDamageHitBox;

    public EnemyJungleBeast_IdleState IdleState { get; private set; }
    public EnemyJungleBeast_JumpState JumpState { get; private set; }
    public EnemyJungleBeast_LookForPlayerState LookForPlayerState { get; private set; }
    public EnemyJungleBeast_MeleeAttackState MeleeAttackState { get; private set; }
    public EnemyJungleBeast_MeleeAttackSecondState MeleeAttackSecondState { get; private set; }
    public EnemyJungleBeast_MoveState MoveState { get; private set; }
    public EnemyJungleBeast_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyJungleBeast_ThrowState ThrowState { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        IdleState = new EnemyJungleBeast_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData);
        JumpState = new EnemyJungleBeast_JumpState(this, StateMachine, Animator.StringToHash(jumpStateData.animationName), jumpStateData);
        LookForPlayerState = new EnemyJungleBeast_LookForPlayerState(this, StateMachine, AnimationConstants.ANIM_PARAM_LOOK_FOR_PLAYER, lookForPlayerStateData);
        MeleeAttackState = new EnemyJungleBeast_MeleeAttackState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK, meleeAttackDamageHitBox, meleeAttackStateData);
        MeleeAttackSecondState = new EnemyJungleBeast_MeleeAttackSecondState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK_SECOND, meleeAttackSecondDamageHitBox, meleeAttackSecondStateData);
        MoveState = new EnemyJungleBeast_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData);
        PlayerDetectedState = new EnemyJungleBeast_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData);
        ThrowState = new EnemyJungleBeast_ThrowState(this, StateMachine, Animator.StringToHash("throw"));
        StateMachine.Initialize(IdleState);
    }

    public override bool ShouldPerformCloseRangeAction()
    {
        if (!MeleeAttackState.IsMeleeAttackReady()) return false;
        if (!AIVision) return false;
        if (!AIVision.HasTarget()) return false;
        float damageHitBoxHalfWidth = meleeAttackDamageHitBox.GetComponent<Collider2D>().bounds.size.x * 2f;
        return Mathf.Abs(Vector3.Distance(transform.position, GetTargetTransform().position))
            <= Mathf.Abs(Vector3.Distance(transform.position, meleeAttackDamageHitBox.transform.position) - damageHitBoxHalfWidth);
    }

    public override bool CheckPlayerInLongRangeAction()
    {
        return base.CheckPlayerInLongRangeAction() && (JumpState.IsJumpReady());
    }

    private void TriggerJump() => JumpState.Jump();
    private void TriggerJumpLanding() => JumpState.JumpLand();
    private void TriggerThrow() => ThrowState.Throw();
    private void TriggerFinishThrow() => ThrowState.FinishThrow();
}
