public class TurnState : IdleState
{
    public TurnState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData) :
        base(enemy, stateMachine, animBoolName, stateData)
    {}

    public override void Enter()
    {
        base.Enter();
        flipAfterIdle = false;
        enemy.ATSM.turnState = this;
    }

    public virtual void FinishTurn()
    {
        Movement.Flip();
    }
}
