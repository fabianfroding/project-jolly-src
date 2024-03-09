public class ShieldEnemy_IdleState : IdleState
{
    ShieldEnemy shieldEnemy;

    public ShieldEnemy_IdleState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData, ShieldEnemy shieldEnemy) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.shieldEnemy = shieldEnemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(shieldEnemy.PlayerDetectedState);
        }
        else if (isIdleTimeOver)
        {
            if (!flipAfterIdle)
            {
                stateMachine.ChangeState(shieldEnemy.MoveState);
            }
        }
    }
}
