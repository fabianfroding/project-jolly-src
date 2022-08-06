public class EnemyHarvester_LookForPlayerState : LookForPlayerState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_LookForPlayerState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(harvester.MoveState);
        }
    }
}
