public class EnemyJungleBeast_ThrowState : State
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    public EnemyJungleBeast_ThrowState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public void Throw()
    {

    }

    public void FinishThrow()
    {
        stateMachine.ChangeState(enemyJungleBeast.LookForPlayerState);
    }
}
