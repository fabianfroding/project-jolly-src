using System;
using UnityEngine;

public class PawnBase : MonoBehaviour
{
    public EntityCore Core { get; protected set; }
    public Animator Animator { get; private set; }

    protected SpriteRenderer spriteRenderer;

    protected Combat Combat => combat ? combat : Core.GetCoreComponent(ref combat);
    protected Combat combat;
    protected Movement Movement => movement ? movement : Core.GetCoreComponent(ref movement);
    protected Movement movement;
    public HealthComponent HealthComponent => healthComponent ? healthComponent : Core.GetCoreComponent(ref healthComponent);
    protected HealthComponent healthComponent;

    public event Action OnDealtDamage;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        Core = GetComponentInChildren<EntityCore>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
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

    public virtual bool IsAlive() => HealthComponent.IsAlive();

    public void BroadcastOnDealtDamage() => OnDealtDamage?.Invoke();

    public int GetFacingDirection() => Movement.FacingDirection;
    #endregion
}
