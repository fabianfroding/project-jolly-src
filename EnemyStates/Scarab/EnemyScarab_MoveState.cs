public class EnemyScarab_MoveState : MoveState
{
    private readonly EnemyScarab enemyScarab;

    public EnemyScarab_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyScarab = (EnemyScarab)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isDetectingWall || !isDetectingLedge)
            stateMachine.ChangeState(enemyScarab.IdleState);
    }
}
