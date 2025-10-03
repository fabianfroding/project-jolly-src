using UnityEngine;

public class ShieldEnemy : EnemyCharacter
{
    public DeadState DeadState { get; private set; }
    public ShieldEnemy_IdleState IdleState { get; private set; }
    public ShieldEnemy_MeleeAttackState MeleeAttackState { get; private set; }
    public ShieldEnemy_MoveState MoveState { get; private set; }
    public ShieldEnemy_PlayerDetectedState PlayerDetectedState { get; private set; }
    public ShieldEnemy_ShieldState ShieldState { get; private set; }
    public ShieldEnemy_StunState StunState { get; private set; }
    public ShieldEnemy_TurnState TurnState { get; private set; }

    [SerializeField] private D_BlockState blockStateData;
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField] private D_StunState stunStateData;

    protected override void Start()
    {
        base.Start();
        DeadState = new DeadState(this, StateMachine, AnimationConstants.ANIM_PARAM_DEAD, deadStateData);
        IdleState = new ShieldEnemy_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        MeleeAttackState = new ShieldEnemy_MeleeAttackState(this, StateMachine, AnimationConstants.ANIM_PARAM_MELEE_ATTACK, meleeAttackDamageHitBox, meleeAttackStateData, this);
        MoveState = new ShieldEnemy_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        PlayerDetectedState = new ShieldEnemy_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        ShieldState = new ShieldEnemy_ShieldState(this, StateMachine, AnimationConstants.ANIM_PARAM_SHIELD, blockStateData, this);
        StunState = new ShieldEnemy_StunState(this, StateMachine, AnimationConstants.ANIM_PARAM_STUN, stunStateData, this);
        TurnState = new ShieldEnemy_TurnState(this, StateMachine, AnimationConstants.ANIM_PARAM_TURN, idleStateData);

        StateMachine.Initialize(IdleState);
    }

    public override void Death()
    {
        base.Death();
        StateMachine.ChangeState(DeadState);
    }
}
