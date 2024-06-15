public class EnemyGundyr_IdleState : IdleState
{
    private EnemyGundyr enemyGundyr;

    public EnemyGundyr_IdleState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData, EnemyGundyr enemyGundyr) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isIdleTimeOver)
        {
            if (!flipAfterIdle)
            {
                stateMachine.ChangeState(enemyGundyr.MoveState);
            }
        }
    }
}
