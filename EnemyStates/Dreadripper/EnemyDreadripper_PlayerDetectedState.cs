public class EnemyDreadripper_PlayerDetectedState : PlayerDetectedState
{
    EnemyDreadripper dreadripper;

    public EnemyDreadripper_PlayerDetectedState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData, EnemyDreadripper dreadripper) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.dreadripper = dreadripper;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.AIVision.TargetPlayerPawn)
            stateMachine.ChangeState(dreadripper.MoveState);
    }
}
