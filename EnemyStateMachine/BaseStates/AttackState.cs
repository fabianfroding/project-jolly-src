using UnityEngine;

public class AttackState : State
{
    protected bool isAnimationFinished;
    protected bool isPlayerInMinAggroRange;
    protected float lastAttackTime;

    protected Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    protected Movement movement;

    public AttackState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName) : base(enemy, stateMachine, animBoolName) {}

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

    public virtual void AttackImpact() {}
}
