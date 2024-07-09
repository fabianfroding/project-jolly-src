using UnityEngine;

public class EnemyDragonWarrior_FlyState : State
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;
    private readonly DragonWarrior_FlyStateData stateData;

    private Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    private Movement movement;

    private CollisionSenses CollisionSenses { get => collisionSenses != null ? collisionSenses : core.GetCoreComponent(ref collisionSenses); }
    private CollisionSenses collisionSenses;

    public EnemyDragonWarrior_FlyState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, DragonWarrior_FlyStateData stateData) : base(enemy, stateMachine, animBoolName)
    {
        enemyDragonWarrior = (EnemyDragonWarrior)enemy;
        this.stateData = stateData;
    }

    public override void Exit()
    {
        base.Exit();
        GameFunctionLibrary.PlayAudioAtPosition(stateData.flyImpactAudioClip, enemyDragonWarrior.transform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Movement.CurrentVelocity.y < 0f &&
            (CollisionSenses.Ground || CollisionSenses.WallUp || CollisionSenses.WallDown || CollisionSenses.CeilingCheck))
        {
            stateMachine.ChangeState(enemyDragonWarrior.IdleState);
        }
    }

    public bool IsFlyReady() => Time.time > EndTime + stateData.flyCooldown;

    public void StartAscend()
    {
        GameFunctionLibrary.PlayAudioAtPosition(stateData.flyStartSound, enemyDragonWarrior.transform.position);
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(40f);
    }

    public void StartDescend()
    {
        if (enemy.HasTarget())
            enemy.transform.position = new Vector2(enemy.GetTargetTransform().position.x, enemy.transform.position.y);
        GameFunctionLibrary.PlayAudioAtPosition(stateData.flyStartDescendAudioClip, enemyDragonWarrior.transform.position);
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(-60f);
    }
}
