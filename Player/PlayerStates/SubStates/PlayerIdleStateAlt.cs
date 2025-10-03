public class PlayerIdleStateAlt : PlayerGroundedStateAlt
{
    public PlayerIdleStateAlt(PlayerCharacter player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityX(0f);
        if (xInput != 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.MoveStateAlt);
        }
    }
}
