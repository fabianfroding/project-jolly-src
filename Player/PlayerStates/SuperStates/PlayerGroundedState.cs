public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    private bool jumpInput;
    private bool isGrounded;
    private bool chargeArrowInput;
    private bool dashInput;
    private bool holdAscendState;
    private bool thunderInput;

    // Null-coalescing operator.
    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
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

        player.JumpState.ResetAmountOfJumpsLeft();
        player.DoubleJumpState?.ResetDoubleJump();
        player.DashState?.ResetCanDash();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        chargeArrowInput = player.InputHandler.ChargeBowInput;
        dashInput = player.InputHandler.DashInput;
        holdAscendState = player.InputHandler.HoldWarpInput;
        thunderInput = player.InputHandler.ThunderInput;

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
        else if (holdAscendState && player.HoldAscendState != null)
        {
            stateMachine.ChangeState(player.HoldAscendState);
        }
        else if (thunderInput && player.ThunderState != null && player.ThunderState.CheckIfCanuseThunder())
        {
            stateMachine.ChangeState(player.ThunderState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
