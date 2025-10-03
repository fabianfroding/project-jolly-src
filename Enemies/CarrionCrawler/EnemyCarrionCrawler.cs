using UnityEngine;

public class EnemyCarrionCrawler : EnemyCharacter
{
    public DeadState DeadState { get; private set; }
    public EnemyCarrionCrawler_MoveState MoveState { get; private set; }
    [SerializeField] private D_DeadState deadStateData;
    [SerializeField] private D_MoveState moveStateData;

    protected override void Start()
    {
        base.Start();

        DeadState = new DeadState(this, StateMachine, AnimationConstants.ANIM_PARAM_DEAD, deadStateData);
        MoveState = new EnemyCarrionCrawler_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);

        StateMachine.Initialize(MoveState);
    }

    public override void Death()
    {
        base.Death();
        StateMachine.ChangeState(DeadState);
    }
}
