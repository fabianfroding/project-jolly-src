using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    protected Combat Combat { get => combat ?? core.GetCoreComponent(ref combat); }
    protected Combat combat;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        CreateSlash();
        player.InputHandler.UseAttackInput();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        int xInput = player.InputHandler.NormInputX;
        Movement.CheckIfShouldFlip(xInput);
        if (xInput == 0 && Movement.CurrentVelocity.x != 0f)
        {
            Movement.SetVelocityX(0f);
        }
        else
        {
            Movement.SetVelocityX(playerStateData.movementVelocity * xInput);
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, 0f);
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_ATTACK, false);
    }

    private void CreateSlash()
    {
        GameObject slash = null;
        Vector2 spawnPos;
        int verticalInput = player.InputHandler.NormInputY;

        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_ATTACK, true);

        if (verticalInput > 0)
        {
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, verticalInput);
            spawnPos = new Vector2(player.GetFireArrowSpawnPosition().x, player.GetFireArrowSpawnPosition().y + verticalInput * 3.5f);
            slash = GameObject.Instantiate(playerStateData.attackSlashPrefabVertical, spawnPos, Quaternion.identity);
        }
        else if (verticalInput < 0 && !CollisionSenses.Ground)
        {
            player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, verticalInput);
            spawnPos = new Vector2(player.GetFireArrowSpawnPosition().x, player.GetFireArrowSpawnPosition().y + verticalInput * 4);
            slash = GameObject.Instantiate(playerStateData.attackSlashPrefabVertical, spawnPos, Quaternion.identity);
            slash.transform.localScale = new Vector2(slash.transform.localScale.x, -slash.transform.localScale.y);
        }
        else
        {
            spawnPos = new Vector2(player.GetFireArrowSpawnPosition().x + Movement.FacingDirection * 3, player.GetFireArrowSpawnPosition().y);
            slash = GameObject.Instantiate(playerStateData.attackSlashPrefabHorizontal, spawnPos, Quaternion.identity);
            slash.transform.localScale = new Vector2(slash.transform.localScale.x * Movement.FacingDirection, slash.transform.localScale.y);
        }

        if (slash != null)
        {
            slash.GetComponent<DamagingObject>().SetSource(player.gameObject);
        }
    }
}
