public class EnemyHarvester_ChargeState : ChargeState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_ChargeState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(harvester.MeleeAttackState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(harvester.LookForPlayerState);
        }
        else if (isChargeTimeOver)
        {
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(harvester.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(harvester.LookForPlayerState);
            }
        }
    }
}
