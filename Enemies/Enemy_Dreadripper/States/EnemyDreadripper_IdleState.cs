public class EnemyDreadripper_IdleState : FlyingIdleState
{
    EnemyDreadripper dreadripper;

    public EnemyDreadripper_IdleState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, EnemyDreadripper dreadripper) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.dreadripper = dreadripper;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.Target != null)
        {
            stateMachine.ChangeState(dreadripper.PlayerDetectedState);
        }
    }
}
