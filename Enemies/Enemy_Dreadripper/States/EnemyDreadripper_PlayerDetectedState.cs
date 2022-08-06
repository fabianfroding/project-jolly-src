public class EnemyDreadripper_PlayerDetectedState : PlayerDetectedState
{
    EnemyDreadripper dreadripper;

    public EnemyDreadripper_PlayerDetectedState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, EnemyDreadripper dreadripper) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.dreadripper = dreadripper;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.Target != null)
        {
            stateMachine.ChangeState(dreadripper.MoveState);
        }
    }
}
