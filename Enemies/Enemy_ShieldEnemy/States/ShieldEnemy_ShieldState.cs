using UnityEngine;

public class ShieldEnemy_ShieldState : BlockState
{
    private readonly ShieldEnemy shieldEnemy;

    public ShieldEnemy_ShieldState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_BlockState stateData, ShieldEnemy shieldEnemy)
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
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isPlayerInMinAggroRange)
        {
            if (shieldEnemy.MeleeAttackState.IsMeleeAttackReady() && Time.time >= StartTime + 1f)
            {
                stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
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
