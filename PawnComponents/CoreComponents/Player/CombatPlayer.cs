using System;
using UnityEngine;

public class CombatPlayer : Combat
{
    private PlayerPawn player;

    public static event Action OnPlayerTakeDamageFromENV;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<PlayerPawn>();
    }

    public override void TakeDamage(Types.DamageData damageData)
    {
        base.TakeDamage(damageData);

        if (HealthComponent.IsAlive() && player.StateMachine.CurrentState != player.TakeDamageState && !HealthComponent.IsInvulnerable())
        {
            HealthComponent.SetInvulnerable(true);

            // Check so that player is not dead to avoid respawning when reviving.
            if (HealthComponent.IsAlive())
            {
                if (!damageData.source.GetComponent<PawnBase>())
                {
                    OnPlayerTakeDamageFromENV?.Invoke();
                }
                else
                {
                    Vector2 dir = GameFunctionLibrary.GetDirectionFromAngle(GameFunctionLibrary.GetAngleBetweenObjects(damageData.source, gameObject));
                    Movement.Knockback(Vector2.zero, 0f, dir.x < 0f ? -1 : 1);
                    player.StateMachine.ChangeState(player.TakeDamageState);
                }
            }
        }
        else if (!HealthComponent.IsAlive())
        {
            player.StateMachine.ChangeState(player.DeadState);
        }
    }
}
