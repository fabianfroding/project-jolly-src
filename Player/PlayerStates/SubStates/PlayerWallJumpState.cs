using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int wallJumpDirection;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, string animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        InstantiateJumpVisuals();
        player.InputHandler.UseJumpInput();
        Movement.SetVelocity(playerStateData.wallJumpVelocity, playerStateData.wallJumpAngle, wallJumpDirection);
        Movement.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_VELOCITY, Movement.CurrentVelocity.y);
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_X_VELOCITY, Mathf.Abs(Movement.CurrentVelocity.x));

        if (Time.time >= startTime + playerStateData.wallJumpTime)
        {
            isAbilityDone = true;
        }

        // jumped during walljumptime - end state / cancel walljump time early, give player control back early
        if (player.InputHandler.JumpInput)
        {
            isAbilityDone = true;
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if (isTouchingWall)
        {
            wallJumpDirection = -Movement.FacingDirection;
        }
        else
        {
            wallJumpDirection = Movement.FacingDirection;
        }
    }

    private void InstantiateJumpVisuals()
    {
        if (collisionSenses.WallBack || collisionSenses.WallFront)
        {
            GameObject tempGO = GameObject.Instantiate(playerStateData.jumpTrailSFXPrefab);
            tempGO.transform.SetParent(player.transform);
            tempGO.transform.position = player.transform.position;

            tempGO = GameObject.Instantiate(playerStateData.jumpSNDPrefab);
            tempGO.transform.position = player.transform.position;
        }
    }
}
