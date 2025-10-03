using UnityEngine;

public class EnemyJungleBeast_MeleeAttackState : MeleeAttackState
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    public EnemyJungleBeast_MeleeAttackState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData)
        : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            if (enemyJungleBeast.MeleeAttackSecondState.IsMeleeAttackReady() && Random.Range(0, 2) <= 0.5f)
                stateMachine.ChangeState(enemyJungleBeast.MeleeAttackSecondState);
            else
                stateMachine.ChangeState(enemyJungleBeast.IdleState);
        }
    }
}
