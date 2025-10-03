using UnityEngine;

public class ShieldEnemy_ShieldState : BlockState
{
    private readonly ShieldEnemy shieldEnemy;

    private AIVisionComponent AIVision => aiVision ? aiVision : core.GetCoreComponent(ref aiVision);
    private AIVisionComponent aiVision;

    public ShieldEnemy_ShieldState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_BlockState stateData, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.shieldEnemy = shieldEnemy;
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
        if ((isPlayerInMinAggroRange || (AIVision && AIVision.TargetPlayerCharacter)))
        {
            if (isPlayerInMinAggroRange && Combat.blockState != Types.EBlockState.E_BlockAbove && shieldEnemy.MeleeAttackState.IsMeleeAttackReady() && Time.time >= StartTime + 1f)
            {
                stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
            }
            else
            {
                if (AIVision && AIVision.TargetPlayerCharacter)
                {
                    Vector2 direction = (AIVision.TargetPlayerCharacter.transform.position - enemy.transform.position).normalized;
                    int yDir = Mathf.RoundToInt(direction.y);
                    if (yDir > 0)
                        Combat.blockState = Types.EBlockState.E_BlockAbove;
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
