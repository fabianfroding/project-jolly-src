using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        player.InputHandler.UseAttackInput();
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
            Movement.SetVelocityX(playerStateData.movementVelocity * 0.5f * xInput);
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        Combat.IsInTriggeredParriedAnimationFrames = false;

        Vector2 attackImpactPosition = GetAttackImpactPosition();
        InstantiateVFXPrefab(attackImpactPosition);
        InstantiateSFXPrefab(attackImpactPosition);

        // Check if damageable is within circle.
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackImpactPosition, playerStateData.attackDamageData.damageRadius);
        foreach (Collider2D collider in detectedObjects)
        {
            IDamageable damageable = collider.GetComponentInChildren<IDamageable>();
            if (damageable != null)
            {
                Types.DamageData damageData = playerStateData.attackDamageData;
                damageData.source = player.gameObject;
                damageData.target = collider.gameObject;
                damageable.TakeDamage(damageData);
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
        player.Animator.SetFloat(AnimationConstants.ANIM_PARAM_Y_INPUT, 0f);
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_ATTACK, false);
    }

    private Vector2 GetAttackImpactPosition()
    {
        CombatPlayer combatPlayer = (CombatPlayer)Combat;
        Vector2 attackImpactPosition = combatPlayer.attackImpactPosition.transform.position;
        int yInput = player.InputHandler.NormInputY;
        if (yInput < 0)
        {
            attackImpactPosition = combatPlayer.attackImpactPositionDown.transform.position;
        }
        else if (yInput > 0)
        {
            attackImpactPosition = combatPlayer.attackImpactPositionUp.transform.position;
        }
        return attackImpactPosition;
    }

    private void InstantiateVFXPrefab(Vector2 attackImpactPosition)
    {
        int yInput = player.InputHandler.NormInputY;
        GameObject vfxInstance;
        if (yInput == 0 && playerStateData.vfxPrefabHorizontal)
        {
            vfxInstance = GameObject.Instantiate(playerStateData.vfxPrefabHorizontal);
            if (Movement.FacingDirection < 0)
            { 
                SpriteRenderer spriteRenderer = vfxInstance.GetComponent<SpriteRenderer>();
                if (spriteRenderer) { spriteRenderer.flipX = true; }
            }
            SetVFXInstancePositionAndDestroy(vfxInstance, attackImpactPosition);
        }
        else if (yInput < 0 && playerStateData.vfxPrefabVertical)
        {
            vfxInstance = GameObject.Instantiate(playerStateData.vfxPrefabVertical);
            SpriteRenderer spriteRenderer = vfxInstance.GetComponent<SpriteRenderer>();
            if (spriteRenderer) { spriteRenderer.flipY = true; }
            if (Movement.FacingDirection > 0) { spriteRenderer.flipX = true; }
            SetVFXInstancePositionAndDestroy(vfxInstance, attackImpactPosition);
        }
        else if (yInput > 0 && playerStateData.vfxPrefabVertical)
        {
            vfxInstance = GameObject.Instantiate(playerStateData.vfxPrefabVertical);
            SpriteRenderer spriteRenderer = vfxInstance.GetComponent<SpriteRenderer>();
            if (Movement.FacingDirection > 0) { spriteRenderer.flipX = true; }
            SetVFXInstancePositionAndDestroy(vfxInstance, attackImpactPosition);
        }
    }

    private void SetVFXInstancePositionAndDestroy(GameObject vfxInstance, Vector2 position)
    {
        vfxInstance.transform.position = position;
        vfxInstance.transform.parent = player.transform;
        GameObject.Destroy(vfxInstance, 0.15f);
    }

    private void InstantiateSFXPrefab(Vector2 attackImpactPosition)
    {
        if (playerStateData.attackDamageData.sfxPrefab)
        {
            GameObject sfxInstance = GameObject.Instantiate(playerStateData.attackDamageData.sfxPrefab);
            sfxInstance.transform.position = attackImpactPosition;
        }
    }
}
