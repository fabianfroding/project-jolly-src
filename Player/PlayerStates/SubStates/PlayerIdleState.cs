public class PlayerIdleState : PlayerGroundedState
{
    private bool interactionInput = false;

    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public PlayerIdleState(PlayerPawn playerPawn, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(playerPawn, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        interactionInput = player.InputHandler.InteractInput;

        Movement.SetVelocityX(0f);
        if (xInput != 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.MoveState);
        }

        if (interactionInput)
        {
            if (player.Interact())
                stateMachine.ChangeState(player.InteractState);
            player.InputHandler.UseInteractInput(); 
        }
    }
}
