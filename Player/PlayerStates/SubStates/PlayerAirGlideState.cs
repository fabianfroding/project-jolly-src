using UnityEngine;

public class PlayerAirGlideState : PlayerAbilityState
{
    private bool airGlideInput;
    private bool isGrounded;
    private float defaultGravityScale;
    private int xInput;
    private Rigidbody2D rigidbody2D;

    public PlayerAirGlideState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();

        rigidbody2D = player.GetComponent<Rigidbody2D>();
        if (rigidbody2D)
        {
            defaultGravityScale = rigidbody2D.gravityScale;
            rigidbody2D.gravityScale = 0f;
            rigidbody2D.velocity = new Vector3(0, playerStateData.airGlideFallVelocity, 0);
        }
        else
        {
            Debug.LogError("PlayerAirGlideState:Enter: Failed to get player Rigidbody2D component.");
        }

        if (playerStateData.airGlideStartSFX)
        {
            GameObject tempGO = GameObject.Instantiate(playerStateData.airGlideStartSFX);
            tempGO.transform.position = player.transform.position;
        }

        player.TriggerOnPlayerEnterAirGlideState();
    }

    public override void Exit()
    {
        base.Exit();

        if (rigidbody2D)
        {
            rigidbody2D.gravityScale = defaultGravityScale;
        }

        if (playerStateData.airGlideEndSFX)
        {
            GameObject tempGO = GameObject.Instantiate(playerStateData.airGlideEndSFX);
            tempGO.transform.position = player.transform.position;
        }

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
            Movement.SetVelocityX(playerStateData.movementVelocity * xInput);
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_X_VELOCITY, Mathf.Abs(Movement.CurrentVelocity.x));
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_VELOCITY, Movement.CurrentVelocity.y);
        }

    }
}
