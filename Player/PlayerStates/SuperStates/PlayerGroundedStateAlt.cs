public class PlayerGroundedStateAlt : PlayerGroundedState
{
    public PlayerGroundedStateAlt(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.JumpStateAlt.ResetJump();
    }

    public override void LogicUpdate()
    {
        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput && player.JumpStateAlt.CanJump())
        {
            stateMachine.ChangeState(player.JumpStateAlt);
        }
    }
}
