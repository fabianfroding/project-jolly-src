using UnityEngine;

public class EnemyGundyr_MeleeAttackState : MeleeAttackState
{
    private EnemyGundyr enemyGundyr;

    public EnemyGundyr_MeleeAttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData, EnemyGundyr enemyGundyr) : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.enemyGundyr = enemyGundyr;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            if (enemyGundyr.MeleeAttackSecondState.IsMeleeAttackReady() && Random.Range(0, 2) <= 0.5f)
                stateMachine.ChangeState(enemyGundyr.MeleeAttackSecondState);
            else
                stateMachine.ChangeState(enemyGundyr.IdleState);
        }
    }
}
