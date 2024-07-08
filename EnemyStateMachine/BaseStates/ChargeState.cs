using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState stateData;
    protected bool isPlayerInMinAggroRange;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;
    protected float chargeEndTime;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public ChargeState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_ChargeState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        isChargeTimeOver = false;
        GameFunctionLibrary.PlayAudioAtPosition(stateData.chargeStartAudioClip, enemy.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
        chargeEndTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time > StartTime + stateData.chargeUpTime)
            Movement.SetVelocityX(stateData.chargeSpeed * Movement.FacingDirection);

        if (Time.time >= StartTime + stateData.chargeDuration)
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

    public bool IsChargeReady() => Time.time >= chargeEndTime + stateData.chargeCooldown;
}
