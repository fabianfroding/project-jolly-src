using System.Diagnostics;

public class StatusEffect_SpeedTalisman : StatusEffect
{
    private Movement targetMovement;

    public override void Initialize(Entity target)
    {
        base.Initialize(target);
        targetMovement = target.gameObject.GetComponentInChildren<Movement>();
        if (!targetMovement)
            return;
        targetMovement.movementSpeed.AddMutableFloatCoefficient(statusEffectName, effectAmount);
    }

    protected override void StatusEffectEnded()
    {
        if (!targetMovement)
            return;
        targetMovement.movementSpeed.RemoveMutableFloatCoefficient(statusEffectName);
        base.StatusEffectEnded();
    }
}
