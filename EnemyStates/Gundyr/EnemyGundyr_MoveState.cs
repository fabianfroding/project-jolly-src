public class EnemyGundyr_MoveState : MoveState
{
    private EnemyGundyr enemyGundyr;

    public EnemyGundyr_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyGundyr enemyGundyr) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyGundyr = enemyGundyr;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
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
