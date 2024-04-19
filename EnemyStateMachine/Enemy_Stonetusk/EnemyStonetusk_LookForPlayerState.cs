public class EnemyStonetusk_LookForPlayerState : LookForPlayerState
{
    private EnemyStonetusk stonetusk;

    public EnemyStonetusk_LookForPlayerState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_LookForPlayerState stateData, EnemyStonetusk stonetusk) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.stonetusk = stonetusk;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(stonetusk.PlayerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(stonetusk.MoveState);
        }
    }
}
