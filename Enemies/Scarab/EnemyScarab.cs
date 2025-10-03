using UnityEngine;

public class EnemyScarab : EnemyCharacter
{
    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;

    public EnemyScarab_IdleState IdleState { get; private set; }
    public EnemyScarab_MoveState MoveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData);
        MoveState = new(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData);
        StateMachine.Initialize(IdleState);
    }
}
