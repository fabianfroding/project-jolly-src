public class ShieldEnemy_TurnState : TurnState
{
    public ShieldEnemy_TurnState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {}

    public override void FinishTurn()
    {
        base.FinishTurn();
        ShieldEnemy shieldEnemy = (ShieldEnemy)enemy;
        stateMachine.ChangeState(shieldEnemy.MoveState);
    }
}
