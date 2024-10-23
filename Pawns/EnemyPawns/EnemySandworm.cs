using UnityEngine;

public class EnemySandworm : EnemyPawn
{
    [SerializeField] private D_IdleState idleStateData;

    public EnemySandworm_IdleState IdleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new EnemySandworm_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData);
        StateMachine.Initialize(IdleState);
    }
}
