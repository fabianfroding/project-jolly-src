public class PlayerMoveState : PlayerGroundedState
{
    protected StatsPlayer StatsPlayer { get => statsPlayer != null ? statsPlayer : core.GetCoreComponent(ref statsPlayer); }
    protected StatsPlayer statsPlayer;

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.CheckIfShouldFlip(xInput);

        Movement.SetVelocityX(Movement.movementSpeed.GetCurrentValue() * xInput);

        if (xInput == 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }
}
