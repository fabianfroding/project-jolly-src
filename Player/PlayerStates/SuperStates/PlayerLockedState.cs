using UnityEngine;

public class PlayerLockedState : PlayerState
{
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerLockedState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, string animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0f);
    }
}
