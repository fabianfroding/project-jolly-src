public class EnemyPlaguetooth_IdleState : IdleState
{
    EnemyPlaguetooth plaguetooth;

    public EnemyPlaguetooth_IdleState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData, EnemyPlaguetooth plaguetooth) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.plaguetooth = plaguetooth;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(plaguetooth.PlayerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(plaguetooth.MoveState);
        }
    }
}
