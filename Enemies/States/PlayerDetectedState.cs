using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetectedState stateData;
    protected bool isPlayerInMinAggroRange;
    protected bool isPlayerInMaxAggroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;
    protected bool isDetectingLedge;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerDetectedState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        performCloseRangeAction = false;
        performLongRangeAction = false;
        Movement.SetVelocityX(0);

        if (stateData.aggroSoundPrefab != null)
        {
            GameObject deathSound = GameObject.Instantiate(stateData.aggroSoundPrefab, enemy.transform.position, Quaternion.identity);
            deathSound.transform.parent = null;
            GameObject.Destroy(deathSound, deathSound.GetComponent<AudioSource>() != null ? deathSound.GetComponent<AudioSource>().clip.length : 0f);
        }
    }

    public override void Exit()
    {
        base.Exit();
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
