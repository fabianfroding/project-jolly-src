public class FlyingMoveState : MoveState
{
    protected bool isDetectingWallUp;
    protected bool isDetectingWallDown;

    public FlyingMoveState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData) : base(enemy, stateMachine, animBoolName, stateData)
    {}

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingWallUp = CollisionSenses.WallUp;
        isDetectingWallDown = CollisionSenses.WallDown;
    }

    public virtual bool IsAtInitialPosition()
    {
        if ((enemy.transform.position.x >= enemy.InitialPosition.x - 1 && enemy.transform.position.x < enemy.InitialPosition.x + 1) &&
            (enemy.transform.position.y >= enemy.InitialPosition.y - 1 && enemy.transform.position.y < enemy.InitialPosition.y + 1))
        {
            return true;
        }
        return false;
    }
}
