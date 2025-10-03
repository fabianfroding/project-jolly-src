using UnityEngine;

public class EnemyDragonWarrior_SlamState : MeleeAttackState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    public EnemyDragonWarrior_SlamState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.enemyDragonWarrior = (EnemyDragonWarrior)enemy;
    }

    public override void AttackImpact()
    {
        base.AttackImpact();
        GameFunctionLibrary.PlayAudioAtPosition(stateData.windupAudioClip, enemyDragonWarrior.transform.position);
        EventBus.Publish(new CameraShakeEvent(0.75f, 0.01f));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
            stateMachine.ChangeState(enemyDragonWarrior.LookForPlayerState);
    }
}
