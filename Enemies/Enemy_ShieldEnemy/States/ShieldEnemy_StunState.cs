public class ShieldEnemy_StunState : StunState
{
    private ShieldEnemy shieldEnemy;

    public ShieldEnemy_StunState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_StunState stateData, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.shieldEnemy = shieldEnemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isStunTimeOver)
        {
            if (performCloseRangeAction)
            {
                stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
            }
            else
            {
                stateMachine.ChangeState(shieldEnemy.IdleState);
            }
        }
    }
}
