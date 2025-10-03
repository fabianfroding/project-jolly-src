public class EnemyDreadripper_PlayerDetectedState : PlayerDetectedState
{
    EnemyDreadripper dreadripper;

    public EnemyDreadripper_PlayerDetectedState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData, EnemyDreadripper dreadripper) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.dreadripper = dreadripper;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.AIVision.TargetPlayerCharacter)
            stateMachine.ChangeState(dreadripper.MoveState);
    }
}
