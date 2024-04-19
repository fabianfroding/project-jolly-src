public class EnemyHarvester_IdleState : IdleState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_IdleState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(harvester.PlayerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(harvester.MoveState);
        }
    }
}
