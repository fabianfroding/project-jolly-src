using UnityEngine;

public class DodgeState : State
{
    protected D_DodgeState stateData;
    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAggroRange;
    protected bool isGrounded;
    protected bool isDodgeOver;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public DodgeState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_DodgeState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAggroRange = enemy.CheckPlayerInMaxAggroRange();
        isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();
        isDodgeOver = false;
        Movement.SetVelocity(stateData.dodgeSpeed, stateData.dodgeAngle, -Movement.FacingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= StartTime + stateData.dodgeTime && isGrounded)
        {
            isDodgeOver = true;
        }
    }
}
