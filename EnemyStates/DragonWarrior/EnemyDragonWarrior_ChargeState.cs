public class EnemyDragonWarrior_ChargeState : ChargeState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_ChargeState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_ChargeState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemyDragonWarrior = (EnemyDragonWarrior)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(enemyDragonWarrior.IdleState);
        }
        else if (isChargeTimeOver)
        {
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(enemyDragonWarrior.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemyDragonWarrior.LookForPlayerState);
            }
        }
    }
}
