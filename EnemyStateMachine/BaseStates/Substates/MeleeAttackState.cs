using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState stateData;
    protected GameObject meleeAttackDamageHitBox;

    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    public MeleeAttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName)
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

        InstantiateSFXPrefab();
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

    public bool IsMeleeAttackReady()
    {
        return Time.time >= lastAttackTime + stateData.meleeAttackCooldown;
    }

    private void InstantiateSFXPrefab()
    {
        if (stateData.damageData.sfxPrefab)
        {
            GameObject sfxInstance = GameObject.Instantiate(stateData.damageData.sfxPrefab);
            if (Movement.FacingDirection > 0)
            {
                SpriteRenderer spriteRenderer = sfxInstance.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = true;
                }
            }
            sfxInstance.transform.position = enemy.transform.position;
        }
    }
}
