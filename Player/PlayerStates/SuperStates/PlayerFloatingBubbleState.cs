using UnityEngine;

public class PlayerFloatingBubbleState : PlayerState
{
    private int xInput;
    private int yInput;

    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public PlayerFloatingBubbleState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D playerRigidbody2D = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody2D)
        {
            playerRigidbody2D.gravityScale = 0f;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        Rigidbody2D playerRigidbody2D = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody2D)
        {
            playerRigidbody2D.gravityScale = player.GetPlayerStateData().defaultGravityScale;
            playerRigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.PlayerController.NormInputX;
        yInput = player.PlayerController.NormInputY;

        Movement.CheckIfShouldFlip(xInput);
        Movement.SetVelocityX(Movement.movementSpeed.GetCurrentValue() * 0.5f * xInput);
        Movement.SetVelocityY(Movement.movementSpeed.GetCurrentValue() * 0.5f * yInput);

        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_X_VELOCITY, Mathf.Abs(Movement.CurrentVelocity.x));
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_VELOCITY, Movement.CurrentVelocity.y);
    }
}
