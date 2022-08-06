using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState stateData;
    protected bool isPlayerInMinAggroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public ChargeState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        isChargeTimeOver = false;
        Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);
        if (Time.time >= StartTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
        isDetectingLedge = CollisionSenses.Ledge;
        isDetectingWall = CollisionSenses.WallFront;
        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
    }
}
