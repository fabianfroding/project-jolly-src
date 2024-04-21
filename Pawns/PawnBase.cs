using System;
using UnityEngine;

public class PawnBase : MonoBehaviour
{
    public PawnCore Core { get; protected set; }
    public Animator Animator { get; private set; }

    public SpriteRenderer SpriteRenderer { get; protected set; }

    protected Combat Combat => combat ? combat : Core.GetCoreComponent(ref combat);
    protected Combat combat;
    public HealthComponent HealthComponent => healthComponent ? healthComponent : Core.GetCoreComponent(ref healthComponent);
    protected HealthComponent healthComponent;
    protected Movement Movement => movement ? movement : Core.GetCoreComponent(ref movement);
    protected Movement movement;

    public event Action OnDealtDamage;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Core = GetComponentInChildren<PawnCore>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start() {}

    protected virtual void Update()
    {
        if (Core)
            Core.LogicUpdate();
    }

    protected virtual void Death() {}

    public virtual void Revive() {}

    public virtual bool IsAlive() => HealthComponent.IsAlive();

    public void BroadcastOnDealtDamage() => OnDealtDamage?.Invoke();
}
