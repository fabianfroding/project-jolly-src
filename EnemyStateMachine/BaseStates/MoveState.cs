public class MoveState : State
{
    protected D_MoveState stateData;
    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isDetectingEnemy;
    protected bool isPlayerInMinAggroRange;

    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected CollisionSenses collisionSenses;
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = CollisionSenses.Ledge;
        isDetectingWall = CollisionSenses.WallFront;
        isDetectingEnemy = CollisionSenses.EnemyFront;
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
    }
}
