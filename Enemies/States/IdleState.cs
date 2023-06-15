using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;
    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMinAggroRange;
    protected float idleTime;

    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public IdleState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0f);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            Movement.Flip();
        }
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
        if (Time.time >= StartTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public void SetFlipAfterIdle(bool flip) => flipAfterIdle = flip;

    protected void SetRandomIdleTime() =>
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
}
