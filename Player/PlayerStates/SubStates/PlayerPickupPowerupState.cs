using UnityEngine;

public class PlayerPickupPowerupState : PlayerLockedState
{
    float defaultGravityScale;

    public PlayerPickupPowerupState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            defaultGravityScale = rb.gravityScale;
            rb.gravityScale = 0f;
        }
        Movement.SetVelocity(1f, new Vector2(0f, 1f));
    }

    public override void Exit()
    {
        base.Exit();
        Rigidbody2D rb = player.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = defaultGravityScale;
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_PICKUP_POWERUP, false);
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_IDLE, true);
        stateMachine.ChangeState(player.IdleState);
    }
}
