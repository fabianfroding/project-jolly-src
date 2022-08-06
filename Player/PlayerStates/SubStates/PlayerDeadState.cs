using System;

public class PlayerDeadState : PlayerLockedState
{
    public static event Action OnPlayerBecomeDead;

    public PlayerDeadState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, string animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        OnPlayerBecomeDead?.Invoke();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(0f);
    }
}
