public class EnemyDruid_PlayerDetectedState : PlayerDetectedState
{
    private readonly EnemyDruid druid;

    public EnemyDruid_PlayerDetectedState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData, EnemyDruid druid) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.druid = druid;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (performLongRangeAction)
        {
            stateMachine.ChangeState(druid.ChargeState);
        }
        else if (!isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(druid.LookForPlayerState);
        }
    }
}
