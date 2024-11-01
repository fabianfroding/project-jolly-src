public class PlayerMoveStateAlt : PlayerGroundedStateAlt
{
    private readonly Player_StateDataAlt stateDataAlt;

    public PlayerMoveStateAlt(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {
        stateDataAlt = player.GetPlayerStateDataAlt();
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

    public override void AnimationTrigger()
    {
        if (!stateDataAlt)
            return;

        GameFunctionLibrary.PlayRandomAudioAtPosition(stateDataAlt.altFormFootstepAudioClip, player.transform.position);
        EventBus.Publish(stateDataAlt.altFormMoveCameraShakeEvent);
    }
}
