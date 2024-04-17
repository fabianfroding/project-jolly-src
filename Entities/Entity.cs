using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityCore Core { get; protected set; }
    public Animator Animator { get; private set; }

    protected SpriteRenderer spriteRenderer;

    protected Combat Combat => combat ? combat : Core.GetCoreComponent(ref combat);
    protected Combat combat;
    protected Movement Movement => movement ? movement : Core.GetCoreComponent(ref movement);
    protected Movement movement;
    public Stats Stats => stats ? stats : Core.GetCoreComponent(ref stats);
    protected Stats stats;

    private List<StatusEffect> statusEffects;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        Core = GetComponentInChildren<EntityCore>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        statusEffects = new List<StatusEffect>();
    }

    protected virtual void Start() {}

    protected virtual void Update()
    {
        if (Core != null) Core.LogicUpdate();
    }
    #endregion

    #region Other Functions

    protected virtual void Death() {}

    public virtual void Revive() {}

    public int GetFacingDirection() => Movement.FacingDirection;

    public void AddStatusEffect(GameObject statusEffectPrefab)
    {
        if (!statusEffectPrefab)
            return;
        StatusEffect statusEffect = GameObject.Instantiate(statusEffectPrefab).GetComponent<StatusEffect>();
        statusEffects.Add(statusEffect);
        statusEffect.OnStatusEffectEnded += RemoveStatusEffect;
        statusEffect.Initialize(this);
    }

    private void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.OnStatusEffectEnded -= RemoveStatusEffect;
        statusEffects.Remove(statusEffect);
    }
    #endregion
}
