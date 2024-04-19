using UnityEngine;

public class EnemyHarvester_MeleeAttackState : MeleeAttackState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_MeleeAttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, Transform attackPosition, D_MeleeAttackState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, attackPosition, stateData)
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
            else if (!isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(harvester.LookForPlayerState);
            }
        }
    }
}
