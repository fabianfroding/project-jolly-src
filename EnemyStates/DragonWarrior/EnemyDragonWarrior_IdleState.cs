public class EnemyDragonWarrior_IdleState : IdleState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_IdleState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyDragonWarrior = (EnemyDragonWarrior)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isIdleTimeOver || enemy.HasTarget())
            enemyDragonWarrior.StateMachine.ChangeState(enemyDragonWarrior.MoveState);
    }
}
