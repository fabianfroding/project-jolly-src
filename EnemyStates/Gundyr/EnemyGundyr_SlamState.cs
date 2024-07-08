using UnityEngine;

public class EnemyGundyr_SlamState : MeleeAttackState
{
    private readonly EnemyGundyr enemyGundyr;

    public EnemyGundyr_SlamState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.enemyGundyr = (EnemyGundyr)enemy;
    }

    public override void AttackImpact()
    {
        base.AttackImpact();
        GameFunctionLibrary.PlayAudioAtPosition(stateData.windupAudioClip, enemyGundyr.transform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
            stateMachine.ChangeState(enemyGundyr.IdleState);
    }
}
