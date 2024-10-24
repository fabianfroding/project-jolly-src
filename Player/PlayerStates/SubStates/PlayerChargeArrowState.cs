using UnityEngine;

public class PlayerChargeArrowState : PlayerAbilityState
{
    public bool IsCharging { get; private set; }
    private float lastChargeArrowTime;

    private PlayerManaComponent PlayerManaComponent { get => playerManaComponent != null ? playerManaComponent : core.GetCoreComponent(ref playerManaComponent); }
    private PlayerManaComponent playerManaComponent;

    public PlayerChargeArrowState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        IsCharging = true;
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
            if (IsCharging)
            {
                player.Animator.SetBool(AnimationConstants.ANIM_PARAM_CHARGE_ARROW, true);
                player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, player.InputHandler.NormInputY);

                if (player.InputHandler.ChargeBowInputRelease)
                {
                    player.Animator.SetBool(AnimationConstants.ANIM_PARAM_CHARGE_ARROW, false);
                    IsCharging = false;
                    lastChargeArrowTime = Time.time;
                    isAbilityDone = true;
                    PlayerManaComponent.ConsumeManaCharge();
                    stateMachine.ChangeState(player.FireArrowState);
                }
            }
            if (CollisionSenses.Ground)
            {
                Movement.SetVelocityX(0f);
            }
        }

        if (player.InputHandler.BarrierInput)
            player.ActivateBarrier();
    }

    public bool CheckIfCanChargeArrow()
    {
        return (Time.time < playerStateData.chargeArrowCooldown || Time.time >= lastChargeArrowTime + playerStateData.chargeArrowCooldown) &&
            PlayerManaComponent == null || PlayerManaComponent.playerManaCharges.Value > 0;
    }
}
