public class EnemyGundyr_IdleState : IdleState
{
    private readonly EnemyGundyr enemyGundyr;

    public EnemyGundyr_IdleState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isIdleTimeOver)
        {
            if (!flipAfterIdle)
            {
                stateMachine.ChangeState(enemyGundyr.MoveState);
            }
        }
    }
}
