public class EnemyDruid_IdleState : IdleState
{
    private readonly EnemyDruid druid;

    public EnemyDruid_IdleState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData, EnemyDruid druid) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(druid.MoveState);
        }
    }
}
