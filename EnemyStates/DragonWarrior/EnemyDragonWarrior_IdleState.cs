public class EnemyDragonWarrior_IdleState : IdleState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_IdleState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData, EnemyDragonWarrior enemyDragonWarrior) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyDragonWarrior = enemyDragonWarrior;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInMinAggroRange)
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
