public class PlayerJumpState : PlayerJumpStateBase
{
    public PlayerJumpState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {
        jumpVFXPrefab = playerStateData.jumpVFXPrefab;
        jumpTrailVFXPrefab = playerStateData.jumpTrailVFXPrefab;
        jumpAudioClip = playerStateData.jumpAudioClip;
        jumpVelocity = playerStateData.jumpVelocity;
    }
}
