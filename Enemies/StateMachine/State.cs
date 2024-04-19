using UnityEngine;

public class State
{
    public float StartTime { get; protected set; }
    public float EndTime { get; protected set; }

    protected FiniteStateMachine stateMachine;
    protected EnemyPawn enemy;
    protected PawnCore core;

    protected int animBoolName;

    public State(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        core = enemy.Core;
        EndTime = Time.time; 
    }

    public virtual void DoChecks() {}

    public virtual void Enter()
    {
        StartTime = Time.time;
        enemy.Animator.SetBool(animBoolName, true);
        DoChecks();
    }

    public virtual void Exit()
    {
        EndTime = Time.time;
        enemy.Animator.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() {}

    public virtual void PhysicsUpdate() {
        DoChecks();
    }
}
