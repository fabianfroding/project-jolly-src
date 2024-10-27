using UnityEngine;

public class PlayerDoubleJumpState : PlayerAbilityState
{
    private bool hasConsumedDoubleJump;
    private bool isTouchingWall;

    private float entryMoveXSpeed = 0f;

    public PlayerDoubleJumpState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseJumpInput();
        ConsumeDoubleJump();

        InstantiateDoubleJumpVisuals();
        player.InAirState.SetIsJumping();

        if (!CollisionSenses.WallFront)
        {
            Movement.SetVelocityY(playerStateData.bounceVerticalVelocity);
            Movement.SetVelocityX(playerStateData.bounceHorizontalVelocity);
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = CollisionSenses.WallFront;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
            return;
        }

        Movement.CheckIfShouldFlip(player.InputHandler.NormInputX);
        Movement.SetVelocityX(playerStateData.bounceHorizontalVelocity * player.InputHandler.NormInputX);

        if (isTouchingWall)
        {
            player.WallJumpState.DetermineWallJumpDirection(true);
            stateMachine.ChangeState(player.WallJumpState);
        }

        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_X_VELOCITY, Mathf.Abs(Movement.CurrentVelocity.x));
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_VELOCITY, Movement.CurrentVelocity.y);
    }

    public bool CanDoubleJump() => !hasConsumedDoubleJump;
    public void ConsumeDoubleJump() => hasConsumedDoubleJump = true;
    public void ResetDoubleJump() => hasConsumedDoubleJump = false;

    private void InstantiateDoubleJumpVisuals() =>
        GameFunctionLibrary.PlayAudioAtPosition(playerStateData.doubleJumpAudioClip, player.transform.position);
}
