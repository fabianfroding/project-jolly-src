public class EnemyJungleBeast_MoveState : MoveState
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    public EnemyJungleBeast_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemyJungleBeast.ShouldPerformCloseRangeAction() || enemyJungleBeast.CheckPlayerInCloseRangeAction() ||
            (enemyJungleBeast.CheckPlayerInMaxAggroRange() && enemyJungleBeast.CheckPlayerInLongRangeAction()))
        {
            stateMachine.ChangeState(enemyJungleBeast.PlayerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            enemyJungleBeast.IdleState.SetFlipAfterIdle(true); // TODO: This should happen automatically.
            stateMachine.ChangeState(enemyJungleBeast.IdleState);
        }
    }
}
