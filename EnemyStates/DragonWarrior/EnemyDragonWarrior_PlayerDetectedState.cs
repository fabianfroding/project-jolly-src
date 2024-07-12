public class EnemyDragonWarrior_PlayerDetectedState : PlayerDetectedState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_PlayerDetectedState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyDragonWarrior = (EnemyDragonWarrior)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemyDragonWarrior.ShouldPerformCloseRangeAction())
        {
            stateMachine.ChangeState(enemyDragonWarrior.SlamState);
            return;
        }

        if (performLongRangeAction)
        {
            if (enemyDragonWarrior.ChargeState.IsChargeReady())
            {
                stateMachine.ChangeState(enemyDragonWarrior.ChargeState);
            }
            else if (enemyDragonWarrior.FlyState.IsFlyReady())
            {
                stateMachine.ChangeState(enemyDragonWarrior.FlyState);
            }
            return;
        }

        if (!isDetectingLedge)
        {
            enemyDragonWarrior.Flip();
            stateMachine.ChangeState(enemyDragonWarrior.MoveState);
        }
    }
}
