using System;
using UnityEngine;

public class CharacterBase : MonoBehaviour, IDamageable
{
    public Animator Animator { get; protected set; }
    public Collider2D Collider2D { get; protected set; }
    public CharacterCore Core { get; protected set; }
    public Rigidbody2D Rigidbody2D { get; protected set; }
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
        Collider2D = GetComponent<Collider2D>();
        Core = GetComponentInChildren<CharacterCore>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start() {}

    public virtual void TakeDamage(Types.DamageData damageData)
    {
        HealthComponent.TakeDamage(damageData);
    }

    public virtual void Death() {}

    public virtual void Revive() {}

    public virtual bool IsAlive() => HealthComponent.IsAlive();

    public void BroadcastOnDealtDamage() => OnDealtDamage?.Invoke();

    public void Flip()
    {
        if (Movement)
            Movement.Flip();
    }

    public bool IsSpriteMirrored()
    {
        if (!SpriteRenderer)
            return false;
        return SpriteRenderer.flipX;
    }
}
