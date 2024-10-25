public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    protected bool isGrounded; // Change to protected if any ability needs to know if the player is grounded.

    protected CollisionSenses CollisionSenses { get => collisionSenses != null ? collisionSenses : core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerAbilityState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = CollisionSenses.Ground;
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone)
        {
            if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
