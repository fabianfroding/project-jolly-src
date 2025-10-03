using UnityEngine;

public class EnemyGundyr_MeleeAttackSecondState : MeleeAttackState
{
    private readonly EnemyGundyr enemyGundyr;

    public EnemyGundyr_MeleeAttackSecondState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.enemyGundyr = (EnemyGundyr)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            if (enemyGundyr.SlamState.IsMeleeAttackReady() && Random.Range(0, 2) <= 0.5f)
                stateMachine.ChangeState(enemyGundyr.SlamState);
            else
                stateMachine.ChangeState(enemyGundyr.IdleState);
        }
    }
}
