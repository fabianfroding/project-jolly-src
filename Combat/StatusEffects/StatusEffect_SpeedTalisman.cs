public class StatusEffect_SpeedTalisman : StatusEffect
{
    private Movement targetMovement;

    public override void Initialize(Combat targetedCombatComponent)
    {
        base.Initialize(targetedCombatComponent);
        targetMovement = targetedCombatComponent.Movement;
        if (!targetMovement)
            return;
        targetMovement.movementSpeed.AddMutableFloatCoefficient(StatusEffectName, effectAmount);
    }

    protected override void StatusEffectEnded()
    {
        if (!targetMovement)
            return;
        targetMovement.movementSpeed.RemoveMutableFloatCoefficient(StatusEffectName);
        base.StatusEffectEnded();
    }
}
