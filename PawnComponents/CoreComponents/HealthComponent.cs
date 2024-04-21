using System;
using UnityEngine;

public class HealthComponent : CoreComponent, IDamageable
{
    [SerializeField] protected int maxHealth;

    [SerializeField] private GameObject damagedParticles;
    [SerializeField] private GameObject damagedSFX;
    [SerializeField] private GameObject hurtSFXPrefab;
    [SerializeField][Range(0f, 1f)] float chanceToPlayHurtSound;

    public int CurrentHealth { get; private set; }

    private bool invulnerable = false; // TODO: This can be made into a Dictionary if there are multiple invulnerability-granting sources.
    private InvulnerabilityIndication invulnerabilityIndication;

    private SpriteRenderer spriteRenderer;
    private Material defaultSpriteMaterial;
    private Material whiteSpriteMaterial;
    private float whiteFlashStartTime = -1f;
    private readonly float whiteFlashDuration = 0.1f;

    private Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    private Combat combat;
    private Movement Movement => Movement ? movement : core.GetCoreComponent(ref movement);
    private Movement movement;
    protected ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    protected ParticleManager particleManager;

    public event Action<int> OnHealthChange;
    public event Action<int> OnMaxHealthChanged;
    public event Action<Types.DamageData> OnDamageTaken;
    public event Action OnHealthDepleted;

    protected override void Awake()
    {
        base.Awake();
        whiteSpriteMaterial = Resources.Load(EditorConstants.RESOURCE_WHITE_FLASH, typeof(Material)) as Material;
    }

    protected override void Start()
    {
        invulnerabilityIndication = GetComponent<InvulnerabilityIndication>();
        spriteRenderer = componentOwner.SpriteRenderer;
        defaultSpriteMaterial = spriteRenderer.material;

        OnMaxHealthChanged?.Invoke(maxHealth);
        SetHealth(maxHealth);
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

    public bool IsAlive() => CurrentHealth > 0;
    public bool IsInvulnerable() => invulnerable;

    public void TakeDamage(Types.DamageData damageData)
    {
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

        OnDamageTaken?.Invoke(damageData);
        PawnBase source = damageData.source.GetComponent<PawnBase>();
        if (source)
            source.BroadcastOnDealtDamage();

        Debug.Log(damageData.target.name + " took " + damageData.damageAmount + " damage from " + damageData.source.name);
    }

    public void DecreaseHealth(int damageAmount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
        if (IsAlive())
        {
            OnHealthChange?.Invoke(CurrentHealth);
        }
        else
        {
            OnHealthDepleted?.Invoke();
            componentOwner.Death();
        }

        if (invulnerabilityIndication)
            invulnerabilityIndication.StartFlash();
    }

    public void IncreaseHealth(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public void SetHealth(int value)
    {
        CurrentHealth = Mathf.Clamp(value, 1, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
    }

    public int GetMaxHealth() => maxHealth;
    public void SetMaxHealth(int value)
    {
        maxHealth = Mathf.Max(1, value);
        OnMaxHealthChanged?.Invoke(maxHealth);
    }

    public void Kill() => DecreaseHealth(maxHealth);

    public void SetInvulnerable(bool newInvulnerable)
    {
        invulnerable = newInvulnerable;
        if (!invulnerable && invulnerabilityIndication)
            invulnerabilityIndication.EndFlash();
    }

    protected virtual void InstantiateTakeDamageVisuals()
    {
        if (ParticleManager && damagedParticles)
        {
            ParticleManager.StartParticlesWithRandomRotation(damagedParticles);
        }

        if (damagedSFX)
        {
            GameObject damagedSFXInstance = Instantiate(damagedSFX);
            damagedSFXInstance.transform.position = transform.position;
        }

        if (hurtSFXPrefab && UnityEngine.Random.Range(0f, 1f) <= chanceToPlayHurtSound)
        {
            GameObject hurtSFX = GameObject.Instantiate(hurtSFXPrefab);
            hurtSFX.transform.position = transform.position;
        }
    }
}
