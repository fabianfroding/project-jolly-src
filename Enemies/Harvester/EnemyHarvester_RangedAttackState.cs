using UnityEngine;

public class EnemyHarvester_RangedAttackState : RangedAttackState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_RangedAttackState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, Transform attackPosition, D_RangedAttackState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
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
