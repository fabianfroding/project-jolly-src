using UnityEngine;

public class PlayerDoubleJumpState : PlayerAbilityState
{
    private bool hasConsumedDoubleJump;

    public PlayerDoubleJumpState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        InstantiateDoubleJumpVisuals();

        player.InputHandler.UseJumpInput();
        Movement.SetVelocityY(playerStateData.doubleJumpVelocity);
        Movement.SetVelocityX(0f); // Experimental.
        isAbilityDone = true;
        ConsumeDoubleJump();
        player.InAirState.SetIsJumping();
    }

    public bool CanDoubleJump() => !hasConsumedDoubleJump;
    public void ConsumeDoubleJump() => hasConsumedDoubleJump = true;
    public void ResetDoubleJump() => hasConsumedDoubleJump = false;

    private void InstantiateDoubleJumpVisuals()
    {
        GameObject tempGO = GameObject.Instantiate(playerStateData.doubleJumpSFX);
        tempGO.transform.position = player.transform.position;
    }
}
