using UnityEditor.Overlays;

public class PlayerLandStateAlt : PlayerGroundedState
{
    Player_StateDataAlt playerStateDataAlt;

    public PlayerLandStateAlt(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName, Player_StateDataAlt playerStateDataAlt)
        : base(player, stateMachine, playerStateData, animBoolName)
    {
        this.playerStateDataAlt = playerStateDataAlt;
    }

    public override void Enter()
    {
        base.Enter();
        Movement.ResetVelocity();
        InstantiateLandVisuals();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveStateAlt);
            }
            else if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleStateAlt);
            }
        }
    }

    private void InstantiateLandVisuals()
    {
        if (collisionSenses.Ground)
        {
            GameFunctionLibrary.PlayAudioAtPosition(playerStateDataAlt.jumpLandAudioClip, player.transform.position);
            EventBus.Publish(playerStateDataAlt.altFormJumpLandCameraShakeEvent);
        }
    }
}
