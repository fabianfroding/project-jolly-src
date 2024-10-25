using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    protected bool hasConsumedJump;

    public PlayerJumpState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        InstantiateJumpVisuals();

        player.InputHandler.UseJumpInput();
        Movement.SetVelocityY(playerStateData.jumpVelocity);
        isAbilityDone = true;
        ConsumeJump();
        player.InAirState.SetIsJumping();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.InputHandler.JumpInput && player.DoubleJumpState != null && player.DoubleJumpState.CanDoubleJump())
        {
            stateMachine.ChangeState(player.DoubleJumpState);
        }

        if (player.InputHandler.BarrierInput)
            player.ActivateBarrier();
    }

    public bool CanJump() => !hasConsumedJump;
    public void ConsumeJump() => hasConsumedJump = true;
    public void ResetJump() => hasConsumedJump = false;

    protected void InstantiateJumpVisuals()
    {
        if (collisionSenses.Ground)
        {
            GameObject tempGO = GameObject.Instantiate(playerStateData.jumpVFXPrefab);
            tempGO.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.15f, player.transform.position.z);

            tempGO = GameObject.Instantiate(playerStateData.jumpTrailVFXPrefab);
            tempGO.transform.SetParent(player.transform);
            tempGO.transform.position = player.transform.position;

            GameFunctionLibrary.PlayAudioAtPosition(playerStateData.jumpAudioClip, player.transform.position);
        }
    }
}
