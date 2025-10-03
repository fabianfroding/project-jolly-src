using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayerState stateData;
    protected bool isPlayerInMinAggroRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;
    protected float lastTurnTime;
    protected int amountOfTurnsDone;
    protected bool turnImmediately;

    protected Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public LookForPlayerState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_LookForPlayerState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isAllTurnsDone = false;
        isAllTurnsTimeDone = false;
        lastTurnTime = StartTime;
        amountOfTurnsDone = 0;
        Movement.SetVelocityX(0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0);

        if (turnImmediately)
        {
            Movement.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
            turnImmediately = false;
        }
        else if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            Movement.Flip();
            lastTurnTime = Time.time;
            amountOfTurnsDone++;
        }

        if (amountOfTurnsDone >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }

        if (Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }
    }

    public void SetTurnImmediately(bool flip) => turnImmediately = flip;

    public bool HasFinishedLookingForPlayer() => isAllTurnsDone || isAllTurnsTimeDone;


}
