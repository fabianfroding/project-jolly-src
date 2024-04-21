using UnityEngine;

public class EnemyPlaguetooth : EnemyPawn
{
    public DeadState DeadState { get; private set; }
    public EnemyPlaguetooth_IdleState IdleState { get; private set; }
    public EnemyPlaguetooth_MeleeAttackState MeleeAttackState { get; private set; }
    public EnemyPlaguetooth_MoveState MoveState { get; private set; }
    public EnemyPlaguetooth_PlayerDetectedState PlayerDetectedState { get; private set; }
    public EnemyPlaguetooth_StunState StunState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private Transform meleeAttackPosition;

    protected override void Start()
    {
        base.Start();

        DeadState = new DeadState(this, StateMachine, AnimationConstants.ANIM_PARAM_DEAD, deadStateData);
        IdleState = new EnemyPlaguetooth_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        MeleeAttackState = new EnemyPlaguetooth_MeleeAttackState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK, meleeAttackDamageHitBox, meleeAttackStateData, this);
        MoveState = new EnemyPlaguetooth_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        PlayerDetectedState = new EnemyPlaguetooth_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        StunState = new EnemyPlaguetooth_StunState(this, StateMachine, AnimationConstants.ANIM_PARAM_STUN, stunStateData, this);

        StateMachine.Initialize(IdleState);
    }

    public override void Death()
    {
        base.Death();
        StateMachine.ChangeState(DeadState);
    }

    // No need to call base.Parried() since it does nothing in Enemy.cs.
    public override void Parried() => StateMachine.ChangeState(StunState);

    protected override bool CanBeParried(Types.DamageData damageData)
    {
        if (base.CanBeParried(damageData))
        {
            StateMachine.ChangeState(StunState);
            return true;
        }
        return false;
    }
}
