using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAirGlideState : PlayerAbilityState
{
    private bool airGlideInput;
    private float defaultGravityScale;
    private int xInput;
    private Rigidbody2D rigidbody2D;

    public PlayerAirGlideState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();

        rigidbody2D = player.GetComponent<Rigidbody2D>();
        if (rigidbody2D)
        {
            defaultGravityScale = rigidbody2D.gravityScale;
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.linearVelocity = new Vector3(0, playerStateData.airGlideFallVelocity, 0);
        }
        else
        {
            Debug.LogError("PlayerAirGlideState:Enter: Failed to get player Rigidbody2D component.");
        }

        GameFunctionLibrary.PlayAudioAtPosition(playerStateData.airGlideStartAudioClip, player.transform.position);

        player.TriggerOnPlayerEnterAirGlideState();
    }

    public override void Exit()
    {
        base.Exit();

        if (rigidbody2D)
        {
            rigidbody2D.gravityScale = defaultGravityScale;
        }

        GameFunctionLibrary.PlayAudioAtPosition(playerStateData.airGlideEndAudioClip, player.transform.position);

        player.TriggerOnPlayerExitAirGlideState();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isGrounded = CollisionSenses.Ground;
        airGlideInput = player.InputHandler.AirGlideInput;
        xInput = player.InputHandler.NormInputX;

        if (isGrounded)
        {
            player.StateMachine.ChangeState(player.IdleState);
        }
        else if (!airGlideInput)
        {
            player.StateMachine.ChangeState(player.InAirState);
        }
        else
        {
            Movement.CheckIfShouldFlip(xInput);
            Movement.SetVelocityX(Movement.movementSpeed.GetCurrentValue() * xInput);
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_X_VELOCITY, Mathf.Abs(Movement.CurrentVelocity.x));
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_VELOCITY, Movement.CurrentVelocity.y);
        }

    }
}
