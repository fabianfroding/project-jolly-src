using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetectedState stateData;
    protected bool isPlayerInMinAggroRange;
    protected bool isPlayerInMaxAggroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;
    protected float aggroSoundResetStartTime = -1f;

    protected CollisionSenses CollisionSenses { get => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement ? movement : core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerDetectedState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        performCloseRangeAction = false;
        performLongRangeAction = false;
        Movement.SetVelocityX(0);

        if (stateData.aggroAudioClip && aggroSoundResetStartTime <= -1f)
        {
            GameFunctionLibrary.PlayAudioAtPosition(stateData.aggroAudioClip, enemy.transform.position);
            aggroSoundResetStartTime = Time.time;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0);
        if (Time.time >= StartTime + stateData.closeRangeActionTime)
        {
            performCloseRangeAction = true;
        }
        if (Time.time >= StartTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }

        if (stateData.aggroAudioClip)
        {
            if (enemy.HasTarget())
                aggroSoundResetStartTime = Time.time;
            if (Time.time > aggroSoundResetStartTime + stateData.aggroSoundResetTime)
            {
                aggroSoundResetStartTime = -1f;
            }
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
        isPlayerInMaxAggroRange = enemy.CheckPlayerInMaxAggroRange();
        isDetectingLedge = CollisionSenses.Ledge;
        performCloseRangeAction = enemy.CheckPlayerInCloseRangeAction();
        performLongRangeAction = enemy.CheckPlayerInLongRangeAction();
    }
}
