public class EnemyScarab_IdleState : IdleState
{
    private readonly EnemyScarab enemyScarab;

    public EnemyScarab_IdleState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData) : 
        base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyScarab = (EnemyScarab)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isIdleTimeOver)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemyScarab.MoveState);
        }
    }
}
