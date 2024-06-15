public class EnemyDruid_LookForPlayerState : LookForPlayerState
{
    private readonly EnemyDruid druid;

    public EnemyDruid_LookForPlayerState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_LookForPlayerState stateData, EnemyDruid druid) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.druid = druid;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(druid.PlayerDetectedState);
        }
        else if (isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(druid.MoveState);
        }
    }
}
