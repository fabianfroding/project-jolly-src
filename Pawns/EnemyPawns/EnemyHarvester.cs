using UnityEngine;

public class EnemyHarvester : EnemyPawn
{
    public EnemyHarvester_IdleState IdleState { get; private set; }
    public EnemyHarvester_MoveState MoveState { get; private set; }
    public EnemyHarvester_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyHarvester_ChargeState ChargeState { get; private set; }
    public EnemyHarvester_LookForPlayerState LookForPlayerState { get; private set; }
    public EnemyHarvester_MeleeAttackState MeleeAttackState { get; private set; }
    public EnemyHarvester_StunState StunState { get; private set; }
    public DeadState DeadState { get; private set; }
    public EnemyHarvester_DodgeState DodgeState { get; private set; }
    public EnemyHarvester_RangedAttackState RangedAttackState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_ChargeState chargeStateData;
    [SerializeField] private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;
    public D_DodgeState dodgeStateData; // Public because it needs to be accessed from other state scripts AND the inspector.
    [SerializeField] D_RangedAttackState rangedAttackStateData;

    [SerializeField] private Transform rangedAttackPosition;

    protected override void Start()
    {
        base.Start();

        MoveState = new EnemyHarvester_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        IdleState = new EnemyHarvester_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        PlayerDetectedState = new EnemyHarvester_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        ChargeState = new EnemyHarvester_ChargeState(this, StateMachine, AnimationConstants.ANIM_PARAM_CHARGE, chargeStateData, this);
        LookForPlayerState = new EnemyHarvester_LookForPlayerState(this, StateMachine, AnimationConstants.ANIM_PARAM_LOOK_FOR_PLAYER, lookForPlayerStateData, this);
        MeleeAttackState = new EnemyHarvester_MeleeAttackState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK, meleeAttackDamageHitBox, meleeAttackStateData, this);
        StunState = new EnemyHarvester_StunState(this, StateMachine, AnimationConstants.ANIM_PARAM_STUN, stunStateData, this);
        DeadState = new DeadState(this, StateMachine, AnimationConstants.ANIM_PARAM_DEAD, deadStateData);
        DodgeState = new EnemyHarvester_DodgeState(this, StateMachine, AnimationConstants.ANIM_PARAM_DODGE, dodgeStateData, this);
        RangedAttackState = new EnemyHarvester_RangedAttackState(this, StateMachine, AnimationConstants.ANIM_PARAM_RANGED_ATTACK, rangedAttackPosition, rangedAttackStateData, this);

        StateMachine.Initialize(MoveState);
    }

    /*public override void TakeDamage(Types.DamageData damageData)
    {
        base.TakeDamage(damageData);
        if (Combat.IsStunned && StateMachine.currentState != StunState)
        {
            StateMachine.ChangeState(StunState);
        }
        else if (CheckPlayerInMinAggroRange())
        {
            StateMachine.ChangeState(RangedAttackState);
        }

        // Not sure what behaviour this block does. I probably left it out for a preference reason.
        /*else if (!CheckPlayerInMinAggroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }*/

    public override void Death()
    {
        base.Death();
        StateMachine.ChangeState(DeadState);
    }
}
