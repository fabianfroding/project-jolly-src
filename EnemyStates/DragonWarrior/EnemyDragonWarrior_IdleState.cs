public class EnemyDragonWarrior_IdleState : IdleState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_IdleState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isIdleTimeOver)
        {
            if (!flipAfterIdle)
            {
                enemyDragonWarrior.StateMachine.ChangeState(enemyDragonWarrior.MoveState);
            }
        }
    }
}
