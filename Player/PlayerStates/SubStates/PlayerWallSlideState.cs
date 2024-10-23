public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        player.JumpState.ResetJump();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            Movement.SetVelocityY(-playerStateData.wallSlideVelocity);
        }

        if (player.InputHandler.BarrierInput)
            player.ActivateBarrier();
    }
}
