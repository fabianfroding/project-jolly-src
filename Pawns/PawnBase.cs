using System;
using UnityEngine;

public class PawnBase : MonoBehaviour
{
    public PawnCore Core { get; protected set; }
    public Animator Animator { get; private set; }

    public SpriteRenderer spriteRenderer { get; protected set; }

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
        Animator = GetComponent<Animator>();
        Core = GetComponentInChildren<PawnCore>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start() {}

    protected virtual void Update()
    {
        if (Core)
            Core.LogicUpdate();
    }
    #endregion

    #region Other Functions

    protected virtual void Death() {}

    public virtual void Revive() {}

    public virtual bool IsAlive() => HealthComponent.IsAlive();

    public void BroadcastOnDealtDamage() => OnDealtDamage?.Invoke();
    #endregion
}
