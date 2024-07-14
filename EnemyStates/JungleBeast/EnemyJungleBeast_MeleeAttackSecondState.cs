using UnityEngine;

public class EnemyJungleBeast_MeleeAttackSecondState : MeleeAttackState
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    public EnemyJungleBeast_MeleeAttackSecondState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData)
        : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
            stateMachine.ChangeState(enemyJungleBeast.IdleState);
    }
}
