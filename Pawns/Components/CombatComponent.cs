using System.Collections.Generic;
using UnityEngine;

public class CombatComponent : ActorComponent
{
    private List<StatusEffect> statusEffects;

    #region Unity Callback Functions
    protected override void Awake()
    {
        statusEffects = new List<StatusEffect>();
    }
    #endregion

    public void AddStatusEffect(GameObject statusEffectPrefab)
    {
        if (!statusEffectPrefab)
            return;
        StatusEffect statusEffect = GameObject.Instantiate(statusEffectPrefab).GetComponent<StatusEffect>();
        statusEffects.Add(statusEffect);
        statusEffect.OnStatusEffectEnded += RemoveStatusEffect;
        statusEffect.Initialize((PawnBase)owningActor);
    }

    private void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.OnStatusEffectEnded -= RemoveStatusEffect;
        statusEffects.Remove(statusEffect);
    }
}
