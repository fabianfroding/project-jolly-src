using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    public PlayerAttackState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

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
        base.LogicUpdate();

        int xInput = player.PlayerController.NormInputX;
        Movement.CheckIfShouldFlip(xInput);
        if (xInput == 0 && Movement.CurrentVelocity.x != 0f)
            Movement.SetVelocityX(0f);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Combat.IsInTriggeredParriedAnimationFrames = false;

        int yInput = player.PlayerController.NormInputY;
        if (yInput < 0 && !CollisionSenses.Ground)
        {
            player.attackDownDamageHitBox.SetActive(true);
        }
        else if (yInput > 0)
        {
            player.attackUpDamageHitBox.SetActive(true);
        }
        else
        { 
            player.attackHorizontalDamageHitBox.SetActive(true);
        }
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
        player.attackHorizontalDamageHitBox.SetActive(false);
        player.attackDownDamageHitBox.SetActive(false);
        player.attackUpDamageHitBox.SetActive(false);
    }
}
