public class EnemyDreadripper_IdleState : FlyingIdleState
{
    EnemyDreadripper dreadripper;

    public EnemyDreadripper_IdleState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData, EnemyDreadripper dreadripper) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.dreadripper = dreadripper;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.AIVision.TargetPlayerCharacter)
            stateMachine.ChangeState(dreadripper.PlayerDetectedState);
    }
}
