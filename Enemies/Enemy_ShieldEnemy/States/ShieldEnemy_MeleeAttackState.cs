using UnityEngine;

public class ShieldEnemy_MeleeAttackState : MeleeAttackState
{
    private ShieldEnemy shieldEnemy;

    public ShieldEnemy_MeleeAttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, Transform attackImpactPosition, D_MeleeAttackState stateData, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName, attackImpactPosition, stateData)
    {
        this.shieldEnemy = shieldEnemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            shieldEnemy.IdleState.SetFlipAfterIdle(false);
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(shieldEnemy.ShieldState);
            }
            else
            {
                stateMachine.ChangeState(shieldEnemy.IdleState);
            }
        }
    }
}
