using UnityEngine;

public class EnemyGundyr_MeleeAttackState : MeleeAttackState
{
    private EnemyGundyr enemyGundyr;

    public EnemyGundyr_MeleeAttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData, EnemyGundyr enemyGundyr) : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.enemyGundyr = enemyGundyr;
    }

    public override void AttackImpact()
    {
        base.AttackImpact();
        if (stateData.windupSFXPrefab)
        {
            GameObject windupSFXPrefab = GameObject.Instantiate(stateData.windupSFXPrefab, enemyGundyr.transform);
            windupSFXPrefab.transform.parent = null;
            windupSFXPrefab.transform.position = enemyGundyr.transform.position;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            stateMachine.ChangeState(enemyGundyr.IdleState);
        }
    }
}
