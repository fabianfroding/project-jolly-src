using UnityEngine;

public class PlayerState
{
    protected PawnCore core;

    protected PlayerPawn player;
    protected PlayerStateMachine stateMachine;
    protected Player_StateData playerStateData;
    protected Player_StateDataAlt playerStateDataAlt;
    protected float startTime;
    protected bool isAnimationFinished;
    protected bool isExitingState;

    private int animBoolName;

    public PlayerState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerStateData = playerStateData;
        this.animBoolName = animBoolName;
        core = player.Core;
        playerStateDataAlt = player.GetPlayerStateDataAlt();
    }

    public virtual void DoChecks() {}

    public virtual void Enter()
    {
        DoChecks();
        player.Animator.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    public virtual void Exit()
    {
        player.Animator.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate() {}

    public virtual void PhysicsUpdate() => DoChecks();

    public virtual void AnimationTrigger() {}

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
