using UnityEngine;

public class PlayerFireArrowState : PlayerAbilityState
{
    public PlayerFireArrowState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_FIRE_ARROW, true);
        FireArrow();
    }

    public override void Exit()
    {
        base.Exit();
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, 0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (CollisionSenses.Ground)
        {
            if (player.InputHandler.NormInputX == 0 && Movement.CurrentVelocity.x != 0f)
                Movement.SetVelocityX(0f);
        }

        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, player.InputHandler.NormInputY);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, 0f);
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_FIRE_ARROW, false);
    }

    private void FireArrow()
    {
        Vector2 inputDir;
        Vector2 spawnPos;
        int verticalInput = player.InputHandler.NormInputY;

        if (verticalInput != 0)
        {
            inputDir = new Vector2(0, verticalInput);
            spawnPos = new Vector2(player.GetFireArrowSpawnPosition().x, player.GetFireArrowSpawnPosition().y + verticalInput);

            // Apply upward velocity if firing arrow down.
            if (verticalInput < 0)
            {
                //Movement.SetVelocityY(+playerStateData.arrowUpwardVelocity);
            }
            else if (verticalInput > 0)
            {
                //Movement.SetVelocityY(-playerStateData.arrowDownwardVelocity);
            }
        }
        else
        {
            inputDir = new Vector2(Movement.FacingDirection, 0);
            spawnPos = new Vector2(player.GetFireArrowSpawnPosition().x + Movement.FacingDirection, player.GetFireArrowSpawnPosition().y);
            if (!CollisionSenses.Ground)
            {
                Movement.SetVelocityX(playerStateData.arrowSidewayVelocity * -Movement.FacingDirection);
            }
        }

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER), LayerMask.NameToLayer("PlayerProjectile"));
        GameObject arrow = GameObject.Instantiate(playerStateData.arrowPrefab, spawnPos, Quaternion.identity);
        Projectile projectile = arrow.GetComponent<Projectile>();

        Types.DamageData damageData = projectile.GetDamageData();
        damageData.source = player.gameObject;
        projectile.SetDamageData(damageData);

        if (verticalInput != 0)
        {
            projectile.SetRotation(new Vector3(0, 0, 90));
        }
        projectile.Init(player.gameObject, inputDir);
    }
}
