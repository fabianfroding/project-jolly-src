public class EnemyPlaguetooth_PlayerDetectedState : PlayerDetectedState
{
    EnemyPlaguetooth plaguetooth;

    public EnemyPlaguetooth_PlayerDetectedState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData, EnemyPlaguetooth plaguetooth) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.plaguetooth = plaguetooth;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(plaguetooth.MeleeAttackState);
        }
        else if (!isPlayerInMaxAggroRange)
        {
            //stateMachine.ChangeState(plaguetooth.LookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            plaguetooth.Core.GetCoreComponent<Movement>().Flip();
            stateMachine.ChangeState(plaguetooth.MoveState);
        }
    }
}
