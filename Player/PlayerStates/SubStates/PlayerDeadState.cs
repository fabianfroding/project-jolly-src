using UnityEngine;

public class PlayerDeadState : PlayerState
{
    protected Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Exit()
    {
        base.Exit();
        player.Revive();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(0f);
        if (Time.time > startTime + playerStateData.deadStateDuration)
            player.StateMachine.ChangeState(player.IdleState);
    }
}
