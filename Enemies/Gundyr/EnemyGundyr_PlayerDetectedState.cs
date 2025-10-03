public class EnemyGundyr_PlayerDetectedState : PlayerDetectedState
{
    private EnemyGundyr enemyGundyr;

    public EnemyGundyr_PlayerDetectedState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyGundyr = (EnemyGundyr)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction && enemyGundyr.MeleeAttackState.IsMeleeAttackReady())
        {
            stateMachine.ChangeState(enemyGundyr.MeleeAttackState);
        }
        else if (performLongRangeAction && enemyGundyr.SlamState.IsMeleeAttackReady())
        {
            stateMachine.ChangeState(enemyGundyr.SlamState);
        }
        else if (!isDetectingLedge)
        {
            enemyGundyr.Core.GetCoreComponent<Movement>().Flip();
            stateMachine.ChangeState(enemyGundyr.MoveState);
        }
    }
}
