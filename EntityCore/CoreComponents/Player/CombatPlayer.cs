using UnityEngine;

public class CombatPlayer : Combat
{
    public GameObject attackImpactPosition;
    public GameObject attackImpactPositionDown;
    public GameObject attackImpactPositionUp;

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
}
