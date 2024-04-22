using UnityEngine;

public class PlayerRespawnState : PlayerState
{
    private float respawnStartTime = -1f;
    private Vector2 currentRespawnPosition = Vector2.zero;

    private HealthComponent HealthComponent => healthComponent ? healthComponent : core.GetCoreComponent(ref healthComponent);
    private HealthComponent healthComponent;

    public PlayerRespawnState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        HealthComponent.SetInvulnerable(true);
        respawnStartTime = Time.time;
        player.InputHandler.enabled = false;
    }

    public override void Exit()
    {
        respawnStartTime = -1f;
        player.transform.position = currentRespawnPosition;
        HealthComponent.SetInvulnerable(false);
        player.StopInvulnerabilityFlash();
        player.InputHandler.enabled = true;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (respawnStartTime > -1f && Time.time > respawnStartTime + playerStateData.respawnDelay)
            stateMachine.ChangeState(player.IdleState);
    }

    public void SetRespawnPosition(Vector2 pos) => currentRespawnPosition = pos;
}
