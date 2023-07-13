public class EnemyPlaguetooth_MoveState : MoveState
{
    private EnemyPlaguetooth plaguetooth;

    public EnemyPlaguetooth_MoveState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyPlaguetooth plaguetooth)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.plaguetooth = plaguetooth;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(plaguetooth.PlayerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            plaguetooth.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(plaguetooth.IdleState);
        }
    }
}
