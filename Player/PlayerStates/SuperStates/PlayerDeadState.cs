using UnityEngine;

public class PlayerDeadState : PlayerState
{
    private bool hasRevivedPlayer = false;

    private HealthComponent HealthComponent { get => healthComponent != null ? healthComponent : core.GetCoreComponent(ref healthComponent); }
    private HealthComponent healthComponent;

    private Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    private Movement movement;

    public PlayerDeadState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        hasRevivedPlayer = false;
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer)
            playerSpriteRenderer.enabled = false;

        HealthComponent.SetInvulnerable(true);
        player.PlayerController.enabled = false;
    }

    public override void Exit()
    {
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer)
            playerSpriteRenderer.enabled = true;

        HealthComponent.SetInvulnerable(false);
        player.PlayerController.enabled = true;

        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(0f);
        if (!hasRevivedPlayer && Time.time > startTime + playerStateData.deadStateDuration)
        {
            hasRevivedPlayer = true;
            player.PlayerDeathSequenceFinish();
        }
    }
}
