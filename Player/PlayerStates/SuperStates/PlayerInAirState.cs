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
    private bool holdAscendState;
    private bool wallJumpCoyoteTime;
    private float startWallJumpCoyoteTime;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;

    private float entryMoveXSpeed = 0f;

    private float bounceOnEnemyTriggerTime;

    protected CollisionSenses CollisionSenses { get => collisionSenses != null ? collisionSenses : core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerInAirState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        entryMoveXSpeed = Movement.movementSpeed.GetCurrentValue();
    }

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

        CheckJumpMultiplier();

        // Bounce logic:
        // 1. Wall bounce: When player starts touching wall, start bounce time window and disable bounce for enemy collision.
        // If bounce input is consumed while on wall and within time window, the perform wall bounce.
        // 2. Enemy bounce: If in air and not touching wall, start time window for enemy bounce.
        // If player collides with enemy while in bounce time window, then perform enemy bounce.

        if (player.DoubleJumpState != null)
        {
            // Wall bounce
            if (player.DoubleJumpState.CanDoubleJump()
                && (isTouchingWall || isTouchingWallBack))
            {
                if (jumpInput)
                {
                    stateMachine.ChangeState(player.DoubleJumpState);
                    return;
                }
            }

            // Enemy bounce
            if (player.DoubleJumpState.CanDoubleJump()
                && jumpInput
                && !isTouchingWall
                && !isTouchingWallBack)
            {
                bounceOnEnemyTriggerTime = Time.time;
                return;
            }
        }

        if (player.InputHandler.AttackInput)
        {
            stateMachine.ChangeState(player.AttackState);
            return;
        }

        if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
            return;
        }

        // wall jumping
        /*if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime) && player.WallJumpState != null)
        {
            StopWallJumpCoyoteTime();
            isTouchingWall = CollisionSenses.WallFront;
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
            return;
        }*/

        if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        else if (chargeBowInput && player.ChargeArrowState.CheckIfCanChargeArrow())
        {
            stateMachine.ChangeState(player.ChargeArrowState);
            return;
        }

        Movement.CheckIfShouldFlip(xInput);
        Movement.SetVelocityX(entryMoveXSpeed * xInput);

        // if touching wall and travelling up quickly, slow player down so it doesnt look like ice  
        if (isTouchingWall && Movement.CurrentVelocity.y > 5f)
        {
            Movement.SetVelocityY(Movement.CurrentVelocity.y * playerStateData.wallFrictionMultiplier);
        }

        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_X_VELOCITY, Mathf.Abs(Movement.CurrentVelocity.x));
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_VELOCITY, Movement.CurrentVelocity.y);
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
            player.JumpState.ConsumeJump();
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

    public bool CanBounceOnEnemy()
    {
        return player.DoubleJumpState != null 
            && player.DoubleJumpState.CanDoubleJump()
            && Time.time < bounceOnEnemyTriggerTime + playerStateData.bounceTriggerTime;
    }
}
