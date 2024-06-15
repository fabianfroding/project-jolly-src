public class EnemyDruid_ChargeState : ChargeState
{
    private readonly EnemyDruid druid;

    private HealthComponent HealthComponent { get => healthComponent != null ? healthComponent : core.GetCoreComponent(ref healthComponent); }
    private HealthComponent healthComponent;

    public EnemyDruid_ChargeState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_ChargeState stateData, EnemyDruid druid) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.druid = druid;
    }

    public override void Enter()
    {
        base.Enter();
        HealthComponent.SetInvulnerable(true);
    }

    public override void Exit()
    {
        base.Exit();
        HealthComponent.SetInvulnerable(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate(); // TODO: Comment this out since it shouldnt rely on a timer
        // TODO: check if charge-up time is done. when it is, do another player check and only start charging if player is seen
        // Instead of using timer, keep charging until reaching player x + offset.

        if (!isDetectingLedge || isDetectingWall)
        {
            stateMachine.ChangeState(druid.LookForPlayerState);
        }
        else if (isChargeTimeOver)
        {
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(druid.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(druid.LookForPlayerState);
            }
        }
    }
}
