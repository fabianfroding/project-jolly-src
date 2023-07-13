using UnityEngine;

public class PlayerChargeArrowState : PlayerAbilityState
{
    public bool isCharging { get; private set; }
    private float lastChargeArrowTime;

    public PlayerChargeArrowState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        isCharging = true;
        player.hasCatchedBoomerang = false;
        startTime = Time.time;
        player.InputHandler.UseChargeBowInput();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_CHARGE_ARROW, true);
    }

    public override void Exit()
    { 
        base.Exit();
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, 0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isCharging)
            {
                player.Animator.SetBool(AnimationConstants.ANIM_PARAM_CHARGE_ARROW, true);
                player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, player.InputHandler.NormInputY);

                if (player.InputHandler.ChargeBowInputRelease)
                {
                    player.Animator.SetBool(AnimationConstants.ANIM_PARAM_CHARGE_ARROW, false);
                    isCharging = false;
                    lastChargeArrowTime = Time.time;
                    isAbilityDone = true;
                    stateMachine.ChangeState(player.FireArrowState);
                }
            }
            if (CollisionSenses.Ground)
            {
                Movement.SetVelocityX(0f);
            }
        }
    }

    public bool CheckIfCanChargeArrow()
    {
        return player.hasCatchedBoomerang || 
            Time.time < playerStateData.chargeArrowCooldown || Time.time >= lastChargeArrowTime + playerStateData.chargeArrowCooldown;
    }
}
