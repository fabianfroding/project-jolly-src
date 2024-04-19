using System;
using UnityEngine;

public class CombatPlayer : Combat
{
    public GameObject attackImpactPosition;
    public GameObject attackImpactPositionDown;
    public GameObject attackImpactPositionUp;

    private PlayerPawn player;

    public static event Action OnPlayerTakeDamageFromENV;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<PlayerPawn>();
    }

    public override void Knockback(Vector2 angle, float strength, int direction)
    {
        if (!isKnockbackActive && HealthComponent.IsAlive())
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

        if (HealthComponent.IsAlive() && player.StateMachine.CurrentState != player.TakeDamageState && !HealthComponent.IsInvulnerable())
        {
            HealthComponent.SetInvulnerable(true);

            // Check so that player is not dead to avoid respawning when reviving.
            if (IsPlayerAlive())
            {
                if (!damageData.source.GetComponent<PawnBase>())
                {
                    OnPlayerTakeDamageFromENV?.Invoke();
                }
                else
                {
                    Vector2 dir = GameFunctionLibrary.GetDirectionFromAngle(GameFunctionLibrary.GetAngleBetweenObjects(damageData.source, gameObject));
                    Knockback(Vector2.zero, 0f, dir.x < 0f ? -1 : 1);
                    player.StateMachine.ChangeState(player.TakeDamageState);
                }
            }
        }
        else if (!HealthComponent.IsAlive())
        {
            player.StateMachine.ChangeState(player.DeadState);
        }
    }

    private bool IsPlayerAlive()
    {
        if (!player)
        {
            Debug.LogError("CombatPlayer::IsPlayerDead: Missing Player component.");
            return false;
        }
        return player.StateMachine.CurrentState != player.DeadState;
    }
}