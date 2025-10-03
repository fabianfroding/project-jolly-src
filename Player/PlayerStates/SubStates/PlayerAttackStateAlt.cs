public class PlayerAttackStateAlt : PlayerAbilityState
{
    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    public PlayerAttackStateAlt(PlayerCharacter player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName)
        : base(player, stateMachine, playerStateData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        int yInput = player.PlayerController.NormInputY;
        if (yInput < 0 && !CollisionSenses.Ground)
        {
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, yInput);
        }
        else if (yInput > 0)
        {
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, yInput);
        }

        player.PlayerController.UseAttackInput();
    }

    public override void Exit()
    {
        base.Exit();
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, 0f);
        DisableHitBoxes();
    }

    public override void LogicUpdate()
    {
        if (isAbilityDone)
        {
            if (isGrounded && Movement.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleStateAlt);
            }
            else
            {
                stateMachine.ChangeState(player.InAirStateAlt);
            }
        }

        int xInput = player.PlayerController.NormInputX;
        Movement.CheckIfShouldFlip(xInput);
        if (xInput == 0 && Movement.CurrentVelocity.x != 0f)
            Movement.SetVelocityX(0f);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        Combat.IsInTriggeredParriedAnimationFrames = false;

        player.attackHorizontalDamageHitBoxAlt1.SetActive(true);

        GameFunctionLibrary.PlayRandomAudioAtPosition(playerStateData.attackDamageData.audioClips, player.transform.position);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;

        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, 0f);
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_ATTACK, false);

        DisableHitBoxes();
    }

    private void DisableHitBoxes()
    {
        player.attackHorizontalDamageHitBoxAlt1.SetActive(false);
    }
}
