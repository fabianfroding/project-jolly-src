using System;
using UnityEngine;

public class HealthComponent : CoreComponent
{
    [SerializeField] private IntReference maxHealth;
    [SerializeField] private AudioClip damagedAudioClip;
    [SerializeField] private AudioClip hurtAudioClip;
    [SerializeField][Range(0f, 1f)] float chanceToPlayHurtSound;

    [SerializeField] private IntReference CurrentHealth;

    private bool invulnerable = false; // TODO: This can be made into a Dictionary if there are multiple invulnerability-granting sources.

    private SpriteRenderer spriteRenderer;
    private Material defaultSpriteMaterial;
    private Material whiteSpriteMaterial;
    private float whiteFlashStartTime = -1f;
    private readonly float whiteFlashDuration = 0.1f;

    private Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    private Combat combat;
    private Movement Movement => Movement ? movement : core.GetCoreComponent(ref movement);
    private Movement movement;

    public event Action<int> OnHealthChange;
    public event Action OnHealthDepleted;

    protected override void Awake()
    {
        base.Awake();
        whiteSpriteMaterial = Resources.Load(EditorConstants.RESOURCE_WHITE_FLASH, typeof(Material)) as Material;
    }

    protected override void Start()
    {
        spriteRenderer = componentOwner.SpriteRenderer;
        defaultSpriteMaterial = spriteRenderer.material;

        //SetHealth(maxHealth.Value);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (whiteFlashStartTime > -1f && spriteRenderer && Time.time > whiteFlashStartTime + whiteFlashDuration)
        {
            whiteFlashStartTime = -1f;
            spriteRenderer.material = defaultSpriteMaterial;
        }
    }

    public bool IsAlive() => CurrentHealth.Value > 0;
    public bool IsInvulnerable() => invulnerable;

    public void TakeDamage(Types.DamageData damageData)
    {
        Debug.Log(componentOwner + " take dmg. INv: " + IsInvulnerable());
        if (IsInvulnerable())
            return;
        if (damageData.source == damageData.target)
            return;
        if (!IsAlive())
            return;

        if (damageData.canBeBlocked && Combat.CheckBlock(damageData.source, damageData.target))
            return;

        if (damageData.knockbackStrength > 0f)
            Movement.ApplyKnockback(damageData);

        InstantiateTakeDamageVisuals();

        DecreaseHealth(damageData.damageAmount);

        if (isActiveAndEnabled)
        {
            spriteRenderer.material = whiteSpriteMaterial;
            whiteFlashStartTime = Time.time;
        }

        PawnBase source = damageData.source.GetComponent<PawnBase>();
        if (source)
            source.BroadcastOnDealtDamage();

        //Debug.Log(damageData.target.name + " took " + damageData.damageAmount + " damage from " + damageData.source.name);
    }

    public IntReference GetCurrenthealth() => CurrentHealth;

    public void DecreaseHealth(int damageAmount)
    {
        CurrentHealth.Value = Mathf.Max(0, CurrentHealth.Value - damageAmount);
        OnHealthChange?.Invoke(CurrentHealth.Value);
        if (!IsAlive())
        {
            OnHealthDepleted?.Invoke();
            componentOwner.Death();
        }
    }

    public void IncreaseHealth(int amount)
    {
        CurrentHealth.Value = Mathf.Min(CurrentHealth.Value + amount, maxHealth.Value);
        OnHealthChange?.Invoke(CurrentHealth.Value);
    }

    public void SetHealth(int value)
    {
        CurrentHealth.Value = Mathf.Clamp(value, 1, maxHealth.Value);
        OnHealthChange?.Invoke(CurrentHealth.Value);
    }

    public IntReference GetMaxHealth() => maxHealth;
    public void SetMaxHealth(int value)
    {
        maxHealth.Value = Mathf.Max(1, value);
    }

    public void Kill() => DecreaseHealth(maxHealth.Value);

    public void SetInvulnerable(bool newInvulnerable) => invulnerable = newInvulnerable;

    protected virtual void InstantiateTakeDamageVisuals()
    {
        GameFunctionLibrary.PlayAudioAtPosition(damagedAudioClip, transform.position);
        if (UnityEngine.Random.Range(0f, 1f) <= chanceToPlayHurtSound)
            GameFunctionLibrary.PlayAudioAtPosition(hurtAudioClip, transform.position);
    }
}
