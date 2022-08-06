using UnityEngine;

public class PlayerFireArrowState : PlayerAbilityState
{
    public PlayerFireArrowState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, string animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_FIRE_ARROW, true);
        FireArrow();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityX(0);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
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
                Movement.SetVelocityY(+playerStateData.arrowUpwardVelocity);
            }
            else if (verticalInput > 0)
            {
                Movement.SetVelocityY(-playerStateData.arrowDownwardVelocity);
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

        GameObject arrow = GameObject.Instantiate(playerStateData.arrowPrefab, spawnPos, Quaternion.identity);

        if (verticalInput != 0)
        {
            arrow.GetComponent<Projectile>().SetRotation(new Vector3(0, 0, 90));
        }
        arrow.GetComponent<Projectile>().Init(player.gameObject, inputDir);
    }
}
