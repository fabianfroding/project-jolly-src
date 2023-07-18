using UnityEngine;

public class ShieldEnemy_ShieldState : BlockState
{
    private readonly ShieldEnemy shieldEnemy;
    private FieldOfView fov;

    public ShieldEnemy_ShieldState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_BlockState stateData, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.shieldEnemy = shieldEnemy;
        fov = enemy.GetComponent<FieldOfView>();
    }

    public override void Enter()
    {
        base.Enter();
        Combat.OnAttackBlocked += AttackBlocked;
    }

    public override void Exit()
    {
        Combat.OnAttackBlocked -= AttackBlocked;
        enemy.Animator.SetFloat(AnimationConstants.ANIM_PARAM_SHIELD_DIR, 0f);
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if ((isPlayerInMinAggroRange || (fov && fov.Target)))
        {
            if (isPlayerInMinAggroRange && !Combat.useAltBlockData && shieldEnemy.MeleeAttackState.IsMeleeAttackReady() && Time.time >= StartTime + 1f)
            {
                stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
            }
            else
            {
                if (fov && fov.Target)
                {
                    Vector2 direction = (fov.Target.transform.position - enemy.transform.position).normalized;
                    int yDir = Mathf.RoundToInt(direction.y);
                    Combat.useAltBlockData = yDir > 0;
                    enemy.Animator.SetFloat(AnimationConstants.ANIM_PARAM_SHIELD_DIR, yDir);
                }
            }
        }
        else
        {
            stateMachine.ChangeState(shieldEnemy.IdleState);
        }
    }

    protected override void AttackBlocked()
    {
        base.AttackBlocked();
        if (stateData.chanceToCounterOnBlock > Random.Range(0f, 1f))
        {
            stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
        }
    }
}
