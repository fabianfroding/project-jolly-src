public class EnemyStonetusk_ChargeState : ChargeState
{
    private EnemyStonetusk Stonetusk;
    
    public EnemyStonetusk_ChargeState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_ChargeState stateData, EnemyStonetusk Stonetusk) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.Stonetusk = Stonetusk;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDetectingWall || !isDetectingLedge)
        {
            Stonetusk.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(Stonetusk.IdleState);
        }

        if (isChargeTimeOver)
        {
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(Stonetusk.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(Stonetusk.MoveState);
            }
        }
    }
}
