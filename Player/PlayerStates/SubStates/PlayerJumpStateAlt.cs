public class PlayerJumpStateAlt : PlayerJumpState
{
    public PlayerJumpStateAlt(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {

    }

    public override void LogicUpdate()
    {
        if (isAbilityDone)
        {
            if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleStateAlt);
            }
            else
            {
                //stateMachine.ChangeState(player.InAirState);
            }
        }
    }
}
