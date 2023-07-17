using UnityEngine;

public class ShieldEnemy_ShieldState : State
{
    private ShieldEnemy shieldEnemy;
    private bool isPlayerInMinAggroRange = false;

    protected Combat Combat { get => combat ?? core.GetCoreComponent(ref combat); }
    protected Combat combat;

    public ShieldEnemy_ShieldState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName)
    {
        this.shieldEnemy = shieldEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        Combat.OnAttackBlocked += AttackBlocked;
        Combat.blockingEnabled = true;
    }

    public override void Exit()
    {
        Combat.blockingEnabled = false;
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

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
    }

    private void AttackBlocked()
    {
        // TODO: Can add some randomness here to prevent every single block from triggering a counter-attack.
        stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
    }
}
