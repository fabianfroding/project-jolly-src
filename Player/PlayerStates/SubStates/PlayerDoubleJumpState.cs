using UnityEngine;

public class PlayerDoubleJumpState : PlayerAbilityState
{
    private bool hasConsumedDoubleJump;

    public PlayerDoubleJumpState(PlayerCharacter player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        InstantiateDoubleJumpVisuals();

        player.PlayerController.UseJumpInput();
        Movement.SetVelocityY(playerStateData.doubleJumpVelocity);
        isAbilityDone = true;
        ConsumeDoubleJump();
        player.InAirState.SetIsJumping();
    }

    public bool CanDoubleJump() => !hasConsumedDoubleJump;
    public void ConsumeDoubleJump() => hasConsumedDoubleJump = true;
    public void ResetDoubleJump() => hasConsumedDoubleJump = false;

    private void InstantiateDoubleJumpVisuals() =>
        GameFunctionLibrary.PlayAudioAtPosition(playerStateData.doubleJumpAudioClip, player.transform.position);
}
