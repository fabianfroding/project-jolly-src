using UnityEngine;

[System.Serializable]
public struct JungleBeast_JumpStateData
{
    public float jumpCooldown;
    public AudioClip jumpStartSound;
    public AudioClip jumpLandingSound;
    public string animationName;
    public string landingAnimationName;
    public GameObject takeOffVFX;
    public GameObject impactVFX;
    public CameraShakeEvent impactCameraShakeEvent;
}

public class EnemyJungleBeast_JumpState : State
{
    private readonly EnemyJungleBeast enemyJungleBeast;
    private JungleBeast_JumpStateData stateData;
    private PlayerPawn cachedPlayerPawnTarget;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float jumpDuration;
    private float jumpTimer;
    private bool isJumping;

    private Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    private Movement movement;

    public EnemyJungleBeast_JumpState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, JungleBeast_JumpStateData stateData)
        : base(enemy, stateMachine, animBoolName)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        cachedPlayerPawnTarget = enemy.AIVision.TargetPlayerPawn;
        isJumping = false;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.GetComponent<Collider2D>().isTrigger = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float normalizedTime = jumpTimer / jumpDuration;

            // Calculate current position based on the quadratic Bezier curve
            Vector2 currentPosition = Vector2.Lerp(Vector2.Lerp(startPosition, GetApex(), normalizedTime), Vector2.Lerp(GetApex(), targetPosition, normalizedTime), normalizedTime);

            enemy.transform.position = currentPosition;

            if (jumpTimer >= jumpDuration)
            {
                isJumping = false;
                Movement.SetVelocityX(0f);
                Movement.SetVelocityY(0f);
                enemy.Animator.SetBool(Animator.StringToHash(stateData.animationName), false);
                enemy.Animator.SetBool(Animator.StringToHash(stateData.landingAnimationName), true);
                EventBus.Publish(stateData.impactCameraShakeEvent);
                // TODO: Get overlaps in enemy collison box and damage player if found.
            }
            return;
        }
    }

    public bool IsJumpReady() => Time.time > EndTime + stateData.jumpCooldown;

    public void Jump()
    {
        enemy.GetComponent<Collider2D>().isTrigger = true; // To prevent enemy getting stuck on player when falling.
        GameFunctionLibrary.InstantiateParticleSystemAtPosition(stateData.takeOffVFX, enemy.transform.position);
        GameFunctionLibrary.PlayAudioAtPosition(stateData.jumpStartSound, enemy.transform.position);

        startPosition = enemy.transform.position;
        targetPosition = cachedPlayerPawnTarget.transform.position;
        jumpDuration = 0.9f; // Adjust this value to control jump speed
        jumpTimer = 0f;
        isJumping = true;
    }

    private Vector2 GetApex()
    {
        // Calculate the apex of the jump
        float apexHeight = Mathf.Max(startPosition.y, targetPosition.y) + 30f;
        float midpointX = (startPosition.x + targetPosition.x) / 2;
        return new Vector2(midpointX, apexHeight);
    }

    public void JumpLand()
    {
        Movement.SetVelocityX(0f);
        Movement.SetVelocityY(0f);
        GameFunctionLibrary.InstantiateParticleSystemAtPosition(stateData.impactVFX, enemy.transform.position);
        GameFunctionLibrary.PlayAudioAtPosition(stateData.jumpLandingSound, enemy.transform.position);
        enemy.Animator.SetBool(Animator.StringToHash(stateData.landingAnimationName), false);
        stateMachine.ChangeState(enemyJungleBeast.IdleState);
    }
}
