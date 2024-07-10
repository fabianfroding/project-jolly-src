using UnityEngine;

[System.Serializable]
public struct DragonWarrior_FlyStateData
{
    public float flyCooldown;
    public AudioClip flyImpactAudioClip;
    public AudioClip flyStartSound;
    public AudioClip flyStartDescendAudioClip;
    public string animationName;
    public string landingAnimationName;
    public GameObject takeOffVFX;
    public GameObject impactVFX;
}

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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Movement.CurrentVelocity.y < 0f &&
            (CollisionSenses.Ground || CollisionSenses.WallUp || CollisionSenses.WallDown || CollisionSenses.CeilingCheck))
        {
            enemy.Animator.SetBool(Animator.StringToHash(stateData.animationName), false);
            enemy.Animator.SetBool(Animator.StringToHash(stateData.landingAnimationName), true);
            EventBus.Publish(new CameraShakeEvent(0.75f, 0.01f));
        }
    }

    public bool IsFlyReady() => Time.time > EndTime + stateData.flyCooldown;

    public void StartAscend()
    {
        GameFunctionLibrary.InstantiateParticleSystemAtPosition(stateData.takeOffVFX, enemy.transform.position);
        GameFunctionLibrary.PlayAudioAtPosition(stateData.flyStartSound, enemy.transform.position);
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(40f);
    }

    public void StartDescend()
    {
        if (enemy.HasTarget())
            enemy.transform.position = new Vector2(enemy.GetTargetTransform().position.x, enemy.transform.position.y);
        GameFunctionLibrary.PlayAudioAtPosition(stateData.flyStartDescendAudioClip, enemy.transform.position);
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(-60f);
    }

    public void OnFinishLandingAnimation()
    {
        GameFunctionLibrary.InstantiateParticleSystemAtPosition(stateData.impactVFX, enemy.transform.position);
        GameFunctionLibrary.PlayAudioAtPosition(stateData.flyImpactAudioClip, enemy.transform.position);
        enemy.Animator.SetBool(Animator.StringToHash(stateData.landingAnimationName), false);
        stateMachine.ChangeState(enemyDragonWarrior.IdleState);
    }
}
