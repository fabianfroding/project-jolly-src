using UnityEngine;

public class PlayerTakeDamageState : PlayerState
{
    private float preTakeDamageTimeScale;
    private float preTakeDamageGravityScale;

    private HealthComponent HealthComponent { get => healthComponent != null ? healthComponent : core.GetCoreComponent(ref healthComponent); }
    private HealthComponent healthComponent;

    private Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    private Movement movement;

    public PlayerTakeDamageState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        preTakeDamageTimeScale = Time.timeScale;
        Time.timeScale = playerStateData.takeDamageTimeScale;
        startTime = Time.unscaledTime;

        Rigidbody2D rigidBody2D = player.GetComponent<Rigidbody2D>();
        if (rigidBody2D)
        {
            preTakeDamageGravityScale = rigidBody2D.gravityScale;
            rigidBody2D.gravityScale = 0f;
        }
    }

    public override void Exit()
    {
        Time.timeScale = preTakeDamageTimeScale;

        Rigidbody2D rigidBody2D = player.GetComponent<Rigidbody2D>();
        if (rigidBody2D)
            rigidBody2D.gravityScale = preTakeDamageGravityScale;

        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Movement.CurrentVelocity.y < 0)
            Movement.SetVelocityY(0);
        if (Time.unscaledTime > startTime + playerStateData.takeDamageDuration)
        {
            if (HealthComponent)
                HealthComponent.SetInvulnerable(false);
            stateMachine.ChangeState(player.InAirState);
        }
    }
}
