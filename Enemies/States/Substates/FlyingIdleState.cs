public class FlyingIdleState : IdleState
{
    public FlyingIdleState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {}

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityY(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityY(0f);
    }
}
