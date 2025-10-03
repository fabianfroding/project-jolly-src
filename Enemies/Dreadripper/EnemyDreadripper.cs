using UnityEngine;

public class EnemyDreadripper : EnemyCharacter
{
    public EnemyDreadripper_MoveState MoveState { get; private set; }
    public EnemyDreadripper_IdleState IdleState { get; private set; }
    public EnemyDreadripper_PlayerDetectedState PlayerDetectedState { get; private set; }

    [SerializeField] protected D_IdleState idleStateData;
    [SerializeField] protected D_MoveState moveStateData;
    [SerializeField] protected D_PlayerDetectedState playerDetectedStateData;

    [SerializeField] protected AudioSource[] idleSound;

    protected override void Start()
    {
        base.Start();

        MoveState = new EnemyDreadripper_MoveState(this, StateMachine, AnimationConstants.ANIM_PARAM_MOVE, moveStateData, this);
        IdleState = new EnemyDreadripper_IdleState(this, StateMachine, AnimationConstants.ANIM_PARAM_IDLE, idleStateData, this);
        PlayerDetectedState = new EnemyDreadripper_PlayerDetectedState(this, StateMachine, AnimationConstants.ANIM_PARAM_PLAYER_DETECTED, playerDetectedStateData, this);
        StateMachine.Initialize(IdleState);
    }
    
    // Triggered by animation event in idle animation.
    private void PlayIdleSound()
    {
        // Only play idle sound if the enemy has no target.
        if (AIVision.TargetPlayerCharacter == null && GameFunctionLibrary.IsGameObjectInCameraView(gameObject))
        {
            // Check so that any idle sound is not already playing.
            for (int i = 0; i < idleSound.Length; i++)
            {
                if (idleSound[i].isPlaying)
                    return;
            }

            if (Random.Range(0, 100) <= 20)
            {
                idleSound[Random.Range(0, idleSound.Length - 1)].Play();
            }
        }
    }
}
