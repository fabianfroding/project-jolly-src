using UnityEngine;

public class PlayerTakeDamageState : PlayerState
{
    private float preTakeDamageTimeScale;

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;

    public PlayerTakeDamageState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        preTakeDamageTimeScale = Time.timeScale;
        Time.timeScale = playerStateData.takeDamageTimeScale;
        startTime = Time.unscaledTime;
    }

    public override void Exit()
    {
        Time.timeScale = preTakeDamageTimeScale;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Movement.CurrentVelocity.y < 0)
            Movement.SetVelocityY(0);
        if (Time.unscaledTime > startTime + playerStateData.takeDamageDuration)
            stateMachine.ChangeState(player.InAirState);
    }
}
