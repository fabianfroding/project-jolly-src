public class EnemyDruid_MoveState : MoveState
{
    private readonly EnemyDruid druid;

    public EnemyDruid_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyDruid druid) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isDetectingWall || !isDetectingLedge)
        {
            druid.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(druid.IdleState);
        }
    }
}
