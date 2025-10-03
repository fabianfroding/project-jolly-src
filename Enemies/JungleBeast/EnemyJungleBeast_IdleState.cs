public class EnemyJungleBeast_IdleState : IdleState
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    public EnemyJungleBeast_IdleState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isIdleTimeOver || enemy.HasTarget())
            stateMachine.ChangeState(enemyJungleBeast.MoveState);
    }
}
