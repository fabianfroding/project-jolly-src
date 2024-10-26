using UnityEngine;

public class PlayerLandStateAlt : PlayerGroundedState
{
    public PlayerLandStateAlt(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {
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
            GameFunctionLibrary.PlayAudioAtPosition(playerStateData.jumpLandAudioClip, player.transform.position);
    }
}
