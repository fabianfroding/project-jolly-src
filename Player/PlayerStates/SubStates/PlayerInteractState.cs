using System.Diagnostics;

public class PlayerInteractState : PlayerGroundedState
{
    private bool advanceInteractionInput = false;

    public PlayerInteractState(
        PlayerPawn player, 
        PlayerStateMachine stateMachine, 
        Player_StateData playerStateData, 
        int animBoolName) : 
        base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        advanceInteractionInput = player.InputHandler.AdvanceInteractionInput;
        if (advanceInteractionInput)
        {
            if (!player.AdvanceInteraction())
            {
                stateMachine.ChangeState(player.IdleState);
                player.EndInteraction();
            }
            player.InputHandler.UseAdvanceInteractionInput();
        }
    }
}
