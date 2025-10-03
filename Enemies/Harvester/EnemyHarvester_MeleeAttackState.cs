using UnityEngine;

public class EnemyHarvester_MeleeAttackState : MeleeAttackState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_MeleeAttackState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData, EnemyHarvester harvester) : 
        base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerInMinAggroRange)
                stateMachine.ChangeState(harvester.PlayerDetectedState);
            else if (!isPlayerInMinAggroRange)
                stateMachine.ChangeState(harvester.LookForPlayerState);
        }
    }
}
