public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected bool jumpInput;
    protected bool isGrounded;
    protected bool chargeArrowInput;
    protected bool dashInput;

    protected CollisionSenses CollisionSenses { get => collisionSenses != null ? collisionSenses : core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerGroundedState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetJump();
        player.DoubleJumpState?.ResetDoubleJump();
        player.DashState?.ResetCanDash();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        chargeArrowInput = player.InputHandler.ChargeBowInput;
        dashInput = player.InputHandler.DashInput;

        if (player.InputHandler.AttackInput)
        {
            stateMachine.ChangeState(player.AttackState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if (player.ChargeArrowState.CheckIfCanChargeArrow() && chargeArrowInput)
        {
            stateMachine.ChangeState(player.ChargeArrowState);
        }
        else if (dashInput && player.DashState != null && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
    }
}
