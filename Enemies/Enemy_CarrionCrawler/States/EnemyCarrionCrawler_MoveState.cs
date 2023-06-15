public class EnemyCarrionCrawler_MoveState : MoveState
{
    public EnemyCarrionCrawler_MoveState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyCarrionCrawler carrionCrawler) : base(enemy, stateMachine, animBoolName, stateData)
    {}

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isDetectingWall || isDetectingEnemy || !isDetectingLedge)
        {
            Movement.Flip();
        }
    }
}
