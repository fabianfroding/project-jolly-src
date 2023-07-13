public class ShieldEnemy_PlayerDetectedState : PlayerDetectedState
{
    private ShieldEnemy shieldEnemy;
    
    public ShieldEnemy_PlayerDetectedState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.shieldEnemy = shieldEnemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
        }
        else if (!isDetectingLedge)
        {
            shieldEnemy.Core.GetCoreComponent<Movement>().Flip();
            stateMachine.ChangeState(shieldEnemy.MoveState);
        }
    }
}
