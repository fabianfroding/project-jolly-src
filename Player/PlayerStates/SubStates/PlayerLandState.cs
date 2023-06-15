using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
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
        {
            if (playerStateData.landSoundPrefab)
            {
                GameObject tempGO = GameObject.Instantiate(playerStateData.landSoundPrefab);
                tempGO.transform.position = player.transform.position;
            }
        }
    }
}
