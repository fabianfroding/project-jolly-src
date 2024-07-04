using UnityEngine;

public class EnemyDragonWarrior_SlamState : MeleeAttackState
{
    private EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_SlamState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData, EnemyDragonWarrior enemyDragonWarrior) : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.enemyDragonWarrior = enemyDragonWarrior;
    }

    public override void AttackImpact()
    {
        base.AttackImpact();
        GameFunctionLibrary.PlayAudioAtPosition(stateData.windupAudioClip, enemyDragonWarrior.transform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
            stateMachine.ChangeState(enemyDragonWarrior.IdleState);
    }
}
