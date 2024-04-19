using UnityEngine;

public class AttackState : State
{
    protected Transform attackPosition;
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAggroRange;
    protected float lastAttackTime;

    protected Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    protected Movement movement;

    public AttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, Transform attackPosition) : base(enemy, stateMachine, animBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void Enter()
    {
        base.Enter();
        isAnimationFinished = false;
        enemy.ATSM.attackState = this;
        Movement.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
        lastAttackTime = Time.time;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0f);
    }

    public virtual void TriggerAttack() {}

    public virtual void TriggerParriable() {}

    public virtual void FinishAttack() => isAnimationFinished = true;
}
