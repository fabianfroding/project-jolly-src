public class EnemyPlaguetooth_StunState : StunState
{
    private EnemyPlaguetooth plaguetooth;

    public EnemyPlaguetooth_StunState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_StunState stateData, EnemyPlaguetooth plaguetooth) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.plaguetooth = plaguetooth;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isStunTimeOver)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(plaguetooth.MeleeAttackState);
            }
            else
            {
                stateMachine.ChangeState(plaguetooth.IdleState);
            }
        }
    }
}
