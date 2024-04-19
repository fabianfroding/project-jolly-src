public class PlayerAttackState : PlayerAbilityState
{
    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    protected CombatPlayer combatPlayer;

    public PlayerAttackState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseAttackInput();
        combatPlayer = (CombatPlayer)Combat;
    }

    public override void Exit()
    {
        base.Exit();
        DisableHitBoxes();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        int xInput = player.InputHandler.NormInputX;
        Movement.CheckIfShouldFlip(xInput);
        if (xInput == 0 && Movement.CurrentVelocity.x != 0f)
            Movement.SetVelocityX(0f);
        else
            Movement.SetVelocityX(Movement.movementSpeed.GetCurrentValue() * 0.5f * xInput);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Combat.IsInTriggeredParriedAnimationFrames = false;

        int yInput = player.InputHandler.NormInputY;
        if (yInput < 0)
            combatPlayer.attackDownDamageHitBox.SetActive(true);
        else if (yInput > 0)
            combatPlayer.attackUpDamageHitBox.SetActive(true);
        else
            combatPlayer.attackHorizontalDamageHitBox.SetActive(true);
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
        combatPlayer.attackHorizontalDamageHitBox.SetActive(false);
        combatPlayer.attackDownDamageHitBox.SetActive(false);
        combatPlayer.attackUpDamageHitBox.SetActive(false);
    }
}
