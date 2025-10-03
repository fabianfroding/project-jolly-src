using UnityEngine;

// TODO: Create base class for this and PlayerInAirState for shared variables and behaviours.
public class PlayerInAirStateAlt : PlayerState
{
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;

    private bool isGrounded;
    private bool coyoteTime;
    private bool isJumping;
    private bool isTouchingWall;

    private float entryMoveXSpeed = 0f;

    private CollisionSenses CollisionSenses { get => collisionSenses != null ? collisionSenses : core.GetCoreComponent(ref collisionSenses); }
    private CollisionSenses collisionSenses;
    private Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    private Movement movement;

    public PlayerInAirStateAlt(PlayerCharacter player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        entryMoveXSpeed = Movement.movementSpeed.GetCurrentValue();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = CollisionSenses.Ground;
        isTouchingWall = CollisionSenses.WallFront;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();

        xInput = player.PlayerController.NormInputX;
        jumpInput = player.PlayerController.JumpInput;
        jumpInputStop = player.PlayerController.JumpInputStop;

        CheckJumpMultiplier();
        // TODO: Should alt form be able to attack in air?
        //if (player.PlayerController.AttackInput)
        //{
            //stateMachine.ChangeState(player.AttackState);
        //}
        if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandStateAlt);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpStateAlt);
        }
        else
        {
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

    public void StartCoyoteTime() => coyoteTime = true;

    public void SetIsJumping() => isJumping = true;
}
