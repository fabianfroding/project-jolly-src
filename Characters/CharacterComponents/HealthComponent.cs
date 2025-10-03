using System;
using UnityEngine;

public class HealthComponent : CoreComponent
{
    [SerializeField] private int maxHealth;
    [SerializeField] private AudioClip damagedAudioClip;
    [SerializeField] private AudioClip hurtAudioClip;
    [SerializeField][Range(0f, 1f)] float chanceToPlayHurtSound;

    [SerializeField] private int CurrentHealth;

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
        HealthCombatEvent healthCombatEvent = new(CurrentHealth, componentOwner.gameObject);
        EventBus.Publish(healthCombatEvent);
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
        //Debug.Log(componentOwner + " take dmg. INv: " + IsInvulnerable());
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

        CharacterBase source = damageData.source.GetComponent<CharacterBase>();
        if (source)
            source.BroadcastOnDealtDamage();

        //Debug.Log(damageData.target.name + " took " + damageData.damageAmount + " damage from " + damageData.source.name);
    }

    public int GetCurrenthealth() => CurrentHealth;

    public void DecreaseHealth(int damageAmount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damageAmount);
        
        OnHealthChange?.Invoke(CurrentHealth);
        
        HealthCombatEvent healthCombatEvent = new(CurrentHealth, componentOwner.gameObject);
        EventBus.Publish(healthCombatEvent);
        
        if (!IsAlive())
        {
            OnHealthDepleted?.Invoke();
            componentOwner.Death();
        }
    }


    public void SetHealth(int value)
    {
        CurrentHealth = Mathf.Clamp(value, 1, maxHealth);
        OnHealthChange?.Invoke(CurrentHealth);
        
        HealthCombatEvent healthCombatEvent = new(CurrentHealth, componentOwner.gameObject);
        EventBus.Publish(healthCombatEvent);
    }

    public int GetMaxHealth() => maxHealth;
    public void SetMaxHealth(int value)
    {
        maxHealth = Mathf.Max(1, value);
    }

    public void Kill() => DecreaseHealth(maxHealth);

    public void SetInvulnerable(bool newInvulnerable) => invulnerable = newInvulnerable;

    protected virtual void InstantiateTakeDamageVisuals()
    {
        GameFunctionLibrary.PlayAudioAtPosition(damagedAudioClip, transform.position);
        if (UnityEngine.Random.Range(0f, 1f) <= chanceToPlayHurtSound)
            GameFunctionLibrary.PlayAudioAtPosition(hurtAudioClip, transform.position);
    }
}
