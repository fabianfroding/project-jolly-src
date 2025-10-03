using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState stateData;
    protected GameObject meleeAttackDamageHitBox;

    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    public MeleeAttackState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.meleeAttackDamageHitBox = meleeAttackDamageHitBox;
    }

    public override void Enter()
    {
        base.Enter();
        Combat.IsInTriggeredParriedAnimationFrames = false;
    }

    public override void Exit()
    {
        base.Exit();
        Combat.IsInTriggeredParriedAnimationFrames = false;
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        Combat.IsInTriggeredParriedAnimationFrames = false;

        if (meleeAttackDamageHitBox)
            meleeAttackDamageHitBox.SetActive(true);

        if (stateData.cameraShakeEvent != null)
            EventBus.Publish(stateData.cameraShakeEvent);

        PlayAttackAudio();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        if (meleeAttackDamageHitBox)
            meleeAttackDamageHitBox.SetActive(false);

        Combat.IsInTriggeredParriedAnimationFrames = false;
    }

    public override void TriggerParriable()
    {
        base.TriggerParriable();
        Combat.IsInTriggeredParriedAnimationFrames = true;
    }

    public bool IsMeleeAttackReady() => Time.time >= lastAttackTime + stateData.meleeAttackCooldown;

    private void PlayAttackAudio() =>
        GameFunctionLibrary.PlayRandomAudioAtPosition(stateData.damageData.audioClips, enemy.transform.position);
}
