using UnityEngine;

public class PlayerInAirState : PlayerState
{
    // Input
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool chargeBowInput;
    private bool dashInput;
    private bool airGlideInput;

    // Checks
    private bool isGrounded;
    private bool coyoteTime; // Allows player to still jump when leaving a ledge for a split second.
    private bool isJumping;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool wallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = CollisionSenses.Ground;
        isTouchingWall = CollisionSenses.WallFront;
        isTouchingWallBack = CollisionSenses.WallBack;

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        chargeBowInput = player.InputHandler.ChargeBowInput;
        dashInput = player.InputHandler.DashInput;
        airGlideInput = player.InputHandler.AirGlideInput;

        CheckJumpMultiplier();

        if (airGlideInput)
        {
            stateMachine.ChangeState(player.AirGlideState);
        }
        else if (player.InputHandler.AttackInput)
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        // wall jumping
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = CollisionSenses.WallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && xInput == Movement.FacingDirection && Movement.CurrentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (chargeBowInput && player.ChargeArrowState.CheckIfCanChargeArrow())
        {
            stateMachine.ChangeState(player.ChargeArrowState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            
            Movement.CheckIfShouldFlip(xInput);
            Movement.SetVelocityX(playerStateData.movementVelocity * xInput);

            // if touching wall and travelling up quickly, slow player down so it doesnt look like ice  
            if (isTouchingWall && Movement.CurrentVelocity.y > 5f)
            {
                Movement.SetVelocityY(Movement.CurrentVelocity.y * playerStateData.wallFrictionMultiplier);
            }

            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_X_VELOCITY, Mathf.Abs(Movement.CurrentVelocity.x));
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_VELOCITY, Movement.CurrentVelocity.y);
        }
    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                Movement.SetVelocityY(Movement.CurrentVelocity.y * playerStateData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
        else if (Movement.CurrentVelocity.y < playerStateData.maxFallVelocity)
        {
            Movement.SetVelocityY(playerStateData.maxFallVelocity);
        }
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerStateData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerStateData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;
}
