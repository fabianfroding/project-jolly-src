public class PlayerMoveStateAlt : PlayerGroundedStateAlt
{
    public PlayerMoveStateAlt(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.CheckIfShouldFlip(xInput);

        Movement.SetVelocityX(Movement.movementSpeed.GetCurrentValue() * xInput);

        if (xInput == 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.IdleStateAlt);
        }
    }
}
