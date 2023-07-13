using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState stateData;

    protected Combat Combat { get => combat ?? core.GetCoreComponent(ref combat); }
    protected Combat combat;

    public MeleeAttackState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, Transform attackPosition, D_MeleeAttackState stateData) : base(enemy, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
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

        // Check if player is within circle.
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.damageData.damageRadius);

        foreach (Collider2D collider in detectedObjects)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Types.DamageData damageData = stateData.damageData;
                damageData.source = enemy.gameObject;
                damageData.target = collider.gameObject;
                damageable.TakeDamage(damageData);
            }
        }
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
        Combat.IsInTriggeredParriedAnimationFrames = false;
    }

    public override void TriggerParriable()
    {
        base.TriggerParriable();
        Combat.IsInTriggeredParriedAnimationFrames = true;
    }
}
