using UnityEngine;

public class StunState : State
{
    protected D_StunState stateData;
    protected bool isStunTimeOver;
    protected bool isGrounded;
    protected bool isMovementStopped;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMinAggroRange;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Combat Combat { get => combat ?? core.GetCoreComponent(ref combat); }
    protected Combat combat;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public StunState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_StunState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = CollisionSenses.Ground;
        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isStunTimeOver = false;
        isMovementStopped = false;

        Movement.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
        Combat.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= StartTime + stateData.stunTime)
        {
            isStunTimeOver = true;
        }
        if (isGrounded && Time.time >= StartTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            isMovementStopped = true;
        }
        Movement.SetVelocityX(0f);
    }
}
