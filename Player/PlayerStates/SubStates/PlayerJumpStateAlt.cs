public class PlayerJumpStateAlt : PlayerJumpStateBase
{
    public PlayerJumpStateAlt(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {
        if (playerStateDataAlt)
        {
            jumpVFXPrefab = playerStateDataAlt.jumpVFXPrefab;
            jumpTrailVFXPrefab = playerStateDataAlt.jumpTrailVFXPrefab;
            jumpAudioClip = playerStateDataAlt.jumpAudioClip;
            jumpVelocity = playerStateDataAlt.jumpVelocity;
        }
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
                stateMachine.ChangeState(player.InAirStateAlt);
            }
        }
    }
}
