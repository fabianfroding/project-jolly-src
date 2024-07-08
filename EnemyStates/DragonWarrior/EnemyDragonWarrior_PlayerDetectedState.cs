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

        if (performCloseRangeAction && enemyDragonWarrior.SlamState.IsMeleeAttackReady())
        {
            stateMachine.ChangeState(enemyDragonWarrior.SlamState);
        }
        else if (performLongRangeAction)
        {
            if (enemyDragonWarrior.ChargeState.IsChargeReady())
            {
                stateMachine.ChangeState(enemyDragonWarrior.ChargeState);
            }
            else if (enemyDragonWarrior.FlyState.IsFlyReady())
            {
                stateMachine.ChangeState(enemyDragonWarrior.FlyState);
            }
        }
        else if (!isDetectingLedge)
        {
            enemyDragonWarrior.Core.GetCoreComponent<Movement>().Flip();
            stateMachine.ChangeState(enemyDragonWarrior.MoveState);
        }
    }
}
