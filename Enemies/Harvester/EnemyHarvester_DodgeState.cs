public class EnemyHarvester_DodgeState : DodgeState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_DodgeState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_DodgeState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDodgeOver)
        {
            if (isPlayerInMaxAggroRange && performCloseRangeAction)
            {
                stateMachine.ChangeState(harvester.MeleeAttackState);
            }
            else if (isPlayerInMaxAggroRange && !performCloseRangeAction)
            {
                stateMachine.ChangeState(harvester.RangedAttackState);
            }
            else if (!isPlayerInMaxAggroRange)
            {
                stateMachine.ChangeState(harvester.LookForPlayerState);
            }

            // TODO: Ranged atk state
        }
    }
}
