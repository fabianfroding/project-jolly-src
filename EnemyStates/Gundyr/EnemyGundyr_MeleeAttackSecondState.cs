using UnityEngine;

public class EnemyGundyr_MeleeAttackSecondState : MeleeAttackState
{
    private EnemyGundyr enemyGundyr;

    public EnemyGundyr_MeleeAttackSecondState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData, EnemyGundyr enemyGundyr) : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.enemyGundyr = enemyGundyr;
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
