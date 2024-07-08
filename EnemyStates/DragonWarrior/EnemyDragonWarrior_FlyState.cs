using UnityEngine;

public class EnemyDragonWarrior_FlyState : State
{
    private readonly EnemyDragonWarrior enemyDragonWarrior;
    private float ascendStartTime;

    private Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    private Movement movement;

    private CollisionSenses CollisionSenses { get => collisionSenses != null ? collisionSenses : core.GetCoreComponent(ref collisionSenses); }
    private CollisionSenses collisionSenses;

    public EnemyDragonWarrior_FlyState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyDragonWarrior = (EnemyDragonWarrior)enemy;
    }

    public override void Enter()
    {
        base.Enter();
        ascendStartTime = -1f;
        GameFunctionLibrary.PlayAudioAtPosition(enemyDragonWarrior.flyStartSound, enemyDragonWarrior.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
        // Perform shockwave.
        GameFunctionLibrary.PlayAudioAtPosition(enemyDragonWarrior.GetFlyImpactAudioClip(), enemyDragonWarrior.transform.position);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (ascendStartTime < 0f && Time.time > StartTime + enemyDragonWarrior.GetFlyStartDelay())
        {
            ascendStartTime = Time.time;
            Movement.SetVelocityX(0f);
            Movement.SetVelocityY(40f);
            return;
        }
        if (Movement.CurrentVelocity.y > 0f && Time.time > ascendStartTime + enemyDragonWarrior.GetFlyStartDescendDelay())
        {
            enemy.transform.position = new Vector2(enemy.AIVision.TargetPlayerPawn.transform.position.x, enemy.transform.position.y);
            Movement.SetVelocityX(0f);
            Movement.SetVelocityY(-60f);
            enemy.Animator.SetBool(Animator.StringToHash("special1"), false);
            enemy.Animator.SetBool(Animator.StringToHash("special2"), true);
            return;
        }
        if ((CollisionSenses.Ground || CollisionSenses.WallUp || CollisionSenses.WallDown || CollisionSenses.CeilingCheck)
            && Movement.CurrentVelocity.y < 0f)
        {
            ascendStartTime = -1f;
            enemy.Animator.SetBool(Animator.StringToHash("special2"), false);
            stateMachine.ChangeState(enemyDragonWarrior.IdleState);
        }
    }

    public bool IsFlyReady() => Time.time > EndTime + enemyDragonWarrior.GetFlyCooldown();
}
