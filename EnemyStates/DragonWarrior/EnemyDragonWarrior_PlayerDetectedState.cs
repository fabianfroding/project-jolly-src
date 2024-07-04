public class EnemyDragonWarrior_PlayerDetectedState : PlayerDetectedState
{
    private EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_PlayerDetectedState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData, EnemyDragonWarrior enemyDragonWarrior) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyDragonWarrior = enemyDragonWarrior;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction && enemyDragonWarrior.SlamState.IsMeleeAttackReady())
        {
            stateMachine.ChangeState(enemyDragonWarrior.SlamState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemyDragonWarrior.ChargeState);
        }
        else if (!isDetectingLedge)
        {
            enemyDragonWarrior.Core.GetCoreComponent<Movement>().Flip();
            stateMachine.ChangeState(enemyDragonWarrior.MoveState);
        }
    }
}
