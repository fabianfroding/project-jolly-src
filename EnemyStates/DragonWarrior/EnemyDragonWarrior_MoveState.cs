public class EnemyDragonWarrior_MoveState : MoveState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyDragonWarrior = (EnemyDragonWarrior)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if ((enemyDragonWarrior.CheckPlayerInMaxAggroRange() && enemyDragonWarrior.CheckPlayerInLongRangeAction()) ||
            (enemyDragonWarrior.CheckPlayerInMinAggroRange() && enemyDragonWarrior.CheckPlayerInCloseRangeAction()))
        {
            enemyDragonWarrior.StateMachine.ChangeState(enemyDragonWarrior.PlayerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            enemyDragonWarrior.IdleState.SetFlipAfterIdle(true); // TODO: This should happen automatically.
            stateMachine.ChangeState(enemyDragonWarrior.IdleState);
        }
    }
}
