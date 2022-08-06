public class EnemyStonetusk_IdleState : IdleState
{
    EnemyStonetusk Stonetusk;

    public EnemyStonetusk_IdleState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, EnemyStonetusk Stonetusk) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.Stonetusk = Stonetusk;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(Stonetusk.PlayerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(Stonetusk.MoveState);
        }
    }
}
