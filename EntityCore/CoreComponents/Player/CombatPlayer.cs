using System;
using UnityEngine;

public class CombatPlayer : Combat
{
    public GameObject attackImpactPosition;
    public GameObject attackImpactPositionDown;
    public GameObject attackImpactPositionUp;

    private Player player;

    public static event Action OnPlayerTakeDamageFromENV;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }

    public override void Knockback(Vector2 angle, float strength, int direction)
    {
        if (!isKnockbackActive && Stats.currentHealth > 0)
        {
            // Player should always have the same knockback force applied.
            Movement.SetVelocity(12.5f, new Vector2(1.2f, 1), direction);
            Movement.CanSetVelocity = false;
            isKnockbackActive = true;
            knockbackStartTime = Time.time;
        }
    }

    public override void TakeDamage(Types.DamageData damageData)
    {
        base.TakeDamage(damageData);

        if (stats.currentHealth > 0)
        {
            Invulnerable = true;

            // Check so that player is not dead to avoid respawning when reviving.
            if (IsPlayerAlive())
            {
                if (damageData.damageType == Types.DamageType.ENVIRONMENT)
                {
                    OnPlayerTakeDamageFromENV?.Invoke();
                }
                else
                {
                    Vector2 dir = TrigonometryUtils.GetDirectionFromAngle(TrigonometryUtils.GetAngleBetweenObjects(damageData.source, gameObject));
                    Knockback(Vector2.zero, 0f, dir.x < 0f ? -1 : 1);
                }

                if (invulnerabilityIndication) { invulnerabilityIndication.StartFlash(); }

                StopCoroutine(ResetInvulnerability());
                StartCoroutine(ResetInvulnerability());
            }
        }
    }

    private bool IsPlayerAlive()
    {
        if (!player)
        {
            Debug.LogError("CombatPlayer::IsPlayerDead: Missing Player component.");
            return false;
        }
        return player.StateMachine.CurrentState != player.DyingState && player.StateMachine.CurrentState != player.DeadState;
    }
}
