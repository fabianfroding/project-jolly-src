public class EnemyDragonWarrior_MoveState : MoveState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyDragonWarrior enemyDragonWarrior) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isDetectingWall || !isDetectingLedge)
        {
            enemyDragonWarrior.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemyDragonWarrior.IdleState);
        }
    }
}
