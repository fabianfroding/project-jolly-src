public class EnemyStonetusk_MoveState : MoveState
{
    private EnemyStonetusk stonetusk;

    public EnemyStonetusk_MoveState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, EnemyStonetusk stonetusk) : base(enemy, stateMachine, animBoolName, stateData)
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
        else if (isDetectingWall || !isDetectingLedge)
        {
            stonetusk.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(stonetusk.IdleState);
        }
    }
}
