public class EnemyJungleBeast_LookForPlayerState : LookForPlayerState
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    private AIVisionComponent AIVisionComponent { get => aIVisionComponent != null ? aIVisionComponent : core.GetCoreComponent(ref aIVisionComponent); }
    private AIVisionComponent aIVisionComponent;

    public EnemyJungleBeast_LookForPlayerState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_LookForPlayerState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (AIVisionComponent.HasTarget())
            stateMachine.ChangeState(enemyJungleBeast.PlayerDetectedState);
        if (HasFinishedLookingForPlayer())
            stateMachine.ChangeState(enemyJungleBeast.MoveState);
    }
}
