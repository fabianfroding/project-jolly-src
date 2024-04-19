using UnityEngine;

public class PlayerDyingState : PlayerLockedState
{
    float defaultGravityScale;

    public PlayerDyingState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
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

        Movement.SetVelocity(0f, Vector2.zero);
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

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocity(0f, Vector2.zero);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_DYING, false);
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_DEAD, true);
        stateMachine.ChangeState(player.DeadState);
    }
}
