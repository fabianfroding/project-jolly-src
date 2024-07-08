public class EnemyGundyr_MoveState : MoveState
{
    private readonly EnemyGundyr enemyGundyr;

    public EnemyGundyr_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyGundyr = (EnemyGundyr)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if ((enemyGundyr.CheckPlayerInMaxAggroRange() && enemyGundyr.CheckPlayerInLongRangeAction()) ||
            (enemyGundyr.CheckPlayerInMinAggroRange() && enemyGundyr.CheckPlayerInCloseRangeAction()))
        {
            stateMachine.ChangeState(enemyGundyr.PlayerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            enemyGundyr.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemyGundyr.IdleState);
        }
    }
}
