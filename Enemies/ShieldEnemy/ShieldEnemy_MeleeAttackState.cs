using UnityEngine;

public class ShieldEnemy_MeleeAttackState : MeleeAttackState
{
    private ShieldEnemy shieldEnemy;

    public ShieldEnemy_MeleeAttackState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
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
