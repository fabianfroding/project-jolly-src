public class EnemyDragonWarrior_LookForPlayerState : LookForPlayerState
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;

    private AIVisionComponent AIVisionComponent { get => aIVisionComponent != null ? aIVisionComponent : core.GetCoreComponent(ref aIVisionComponent); }
    private AIVisionComponent aIVisionComponent;

    public EnemyDragonWarrior_LookForPlayerState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_LookForPlayerState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyDragonWarrior = (EnemyDragonWarrior)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (AIVisionComponent.HasTarget())
            enemyDragonWarrior.StateMachine.ChangeState(enemyDragonWarrior.PlayerDetectedState);
        if (HasFinishedLookingForPlayer())
            enemyDragonWarrior.StateMachine.ChangeState(enemyDragonWarrior.MoveState);
    }
}
