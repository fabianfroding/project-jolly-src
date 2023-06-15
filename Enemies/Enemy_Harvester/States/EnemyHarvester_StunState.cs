public class EnemyHarvester_StunState : StunState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_StunState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_StunState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isStunTimeOver)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(harvester.MeleeAttackState);
            }
            else if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(harvester.ChargeState);
            }
            else
            {
                harvester.LookForPlayerState.SetTurnImmediately(true);
                stateMachine.ChangeState(harvester.LookForPlayerState);
            }
        }
    }
}
