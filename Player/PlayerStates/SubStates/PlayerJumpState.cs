using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountOfJumpsLeft;

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, string animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {
        amountOfJumpsLeft = playerStateData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        InstantiateJumpVisuals();

        player.InputHandler.UseJumpInput();
        Movement.SetVelocityY(playerStateData.jumpVelocity);
        isAbilityDone = true;
        DecreaseAmountOfJumpsLeft();
        player.InAirState.SetIsJumping();
    }

    public bool CanJump() => amountOfJumpsLeft > 0;

    public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerStateData.amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;

    private void InstantiateJumpVisuals()
    {
        if (collisionSenses.Ground)
        {
            GameObject tempGO = GameObject.Instantiate(playerStateData.jumpSFXPrefab);
            tempGO.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.15f, player.transform.position.z);

            tempGO = GameObject.Instantiate(playerStateData.jumpTrailSFXPrefab);
            tempGO.transform.SetParent(player.transform);
            tempGO.transform.position = player.transform.position;

            tempGO = GameObject.Instantiate(playerStateData.jumpSNDPrefab);
            tempGO.transform.position = player.transform.position;
        }
    }
}
