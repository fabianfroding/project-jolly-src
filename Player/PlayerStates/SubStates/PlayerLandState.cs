using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(PlayerCharacter player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

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
                stateMachine.ChangeState(player.MoveState);
            }
            else if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    private void InstantiateLandVisuals()
    {
        if (collisionSenses.Ground)
            GameFunctionLibrary.PlayAudioAtPosition(playerStateData.jumpLandAudioClip, player.transform.position);
    }
}
