using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Combat : CoreComponent, IDamageable
{
    [SerializeField] private GameObject damagedParticles;
    [SerializeField] private GameObject damagedSFX;
    protected Material matDefault;
    protected Material matWhite;
    protected SpriteRenderer spriteRenderer;

    [Header("Stun Settings")]
    [Tooltip("Check to allow the entity to recieve stun damage.")]
    [SerializeField] protected bool stunnable = false;
    [Tooltip("The time-window in which an enity can get stunned after it has become vulnerable to stun damage.")]
    [SerializeField] protected float stunTimeWindow = 2f;
    [Tooltip("The amount of hits/damage before the entity can get stunned.")]
    [SerializeField] protected int stunResistance = 3;

    [Header("Parry Settings")]
    [Tooltip("Check to allow an enemy to be parried. This field is for Enemies only. " +
        "Requires setup of appropriate animation parameters TriggerParriable and Trigger Attack, " +
        "which represents the time-window that the enemy can be parried.")]
    [SerializeField] protected bool canBeParried = false;

    [Header("Block Settings")]
    [SerializeField] public bool canBlock = false;
    [SerializeField] public List<Types.BlockData> blockData;
    [Tooltip("Alternative block data that is disabled by default but can be toggled in various circumstances such as top or behind.")]
    [SerializeField] public List<Types.BlockData> blockDataAlt;
    public bool useAltBlockData = false;
    public GameObject blockOriginPosition;
    public bool blockingEnabled = false;

    // Time-window between the attacking-part of a melee attack animation.
    public bool IsInTriggeredParriedAnimationFrames { get; set; }

    public bool IsStunned { get; protected set; }
    public float LastStunDamageTime { get; protected set; }
    protected float currentStunResistance;

    [Header("Spawned Objects")]
    public GameObject parriedSoundPrefab;

    protected HealthComponent HealthComponent => healthComponent ? healthComponent : core.GetCoreComponent(ref healthComponent);
    protected CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses);
    public Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    protected ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);

    protected HealthComponent healthComponent;
    protected CollisionSenses collisionSenses;
    protected Movement movement;
    protected ParticleManager particleManager;

    private List<StatusEffect> statusEffects;

    public event Action OnDamaged;
    public event Action OnAttackBlocked;

    protected override void Awake()
    {
        base.Awake();

        matWhite = Resources.Load(EditorConstants.RESOURCE_WHITE_FLASH, typeof(Material)) as Material;
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        if (!spriteRenderer) { Debug.LogError("Combat::Awake: Could not find SpriteRenderer component."); }
        matDefault = spriteRenderer.material;

        statusEffects = new List<StatusEffect>();
    }

    public virtual void TakeDamage(Types.DamageData damageData)
    {
        if (HealthComponent.IsInvulnerable())
            return;
        if (damageData.source == damageData.target)
            return;

        if (CheckBlock(damageData.source, damageData.target))
        {
            Debug.Log(damageData.target.name + " blocked attack from " + damageData.source.name);
            OnAttackBlocked?.Invoke();
            return;
        }
        
        if (HealthComponent.IsAlive())
        {
            Debug.Log(damageData.target.name + " took " + damageData.damageAmount + " damage from " + damageData.source.name);
            HealthComponent.TakeDamage(damageData);
            OnDamaged?.Invoke();

            InstantiateTakeDamageVisuals();

            // Not sure but I think this is solely used for players rage bar mechanic. Should be moved elsewhere.
            if (damageData.source.CompareTag(EditorConstants.TAG_PLAYER) && !gameObject.CompareTag(EditorConstants.TAG_PLAYER))
            {
                PawnBase source = damageData.source.GetComponent<PawnBase>();
                if (source)
                {
                    source.BroadcastOnDealtDamage();
                }
            }

            if (isActiveAndEnabled)
            {
                StopCoroutine(FlashWhiteMaterial());
                StartCoroutine(FlashWhiteMaterial(0.1f));
            }
            
            Movement.ApplyKnockback(damageData);
        }
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
    }

    public void SetCurrentStunResistance(float amount) => currentStunResistance = amount;
    public void ApplyStunDamage(float amount)
    {
        if (stunnable)
        {
            LastStunDamageTime = Time.time;
            currentStunResistance -= amount;
            if (currentStunResistance <= 0)
            {
                IsStunned = true;
            }
        }
    }
    public virtual int GetStunResistance() => stunResistance;

    public void ResetStunResistance()
    {
        IsStunned = false;
        currentStunResistance = GetStunResistance();
    }

    public void CheckStunRecoveryTime()
    {
        if (Time.time >= LastStunDamageTime + stunTimeWindow)
        {
            ResetStunResistance();
        }
    }

    public bool CanBeParried()
    {
        IParriable parriable = GetComponentInParent<IParriable>();
        if (parriable != null)
        {
            if (IsInTriggeredParriedAnimationFrames)
            {
                parriable.Parried();
                InstantiateParryVisuals();
                return true;
            }
        }
        return false;
    }

    protected void InstantiateParryVisuals()
    {
        if (parriedSoundPrefab != null)
        {
            GameObject parrySound = Instantiate(parriedSoundPrefab);
            parrySound.transform.position = transform.position;
        }

        // TODO: Instantiate parrySFXPrefab.
    }

    protected bool CheckBlock(GameObject source, GameObject target)
    {
        if (!blockingEnabled) { return false; }

        float angle = Vector2.SignedAngle(transform.right, 
            source.transform.position - (blockOriginPosition ? blockOriginPosition.transform.position : target.transform.position));
        
        foreach (Types.BlockData block in useAltBlockData ? blockDataAlt : blockData)
        {
            // Check if the angle is within the defined blocking zone.
            if (angle >= block.minAngle && angle <= block.maxAngle) { return true; }
        }

        return false;
    }

    protected virtual IEnumerator FlashWhiteMaterial(float delay = 0f)
    {
        spriteRenderer.material = matWhite;
        yield return new WaitForSeconds(delay);
        spriteRenderer.material = matDefault;
    }

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 forwardDirection = transform.right;
        Vector2 blockOriginPos = blockOriginPosition ? blockOriginPosition.transform.position : transform.position;
        Handles.color = new Color(0.15f, 0.15f, 1f, 0.35f);
        foreach (Types.BlockData block in blockData)
        {
            if (!block.showDebugVisuals) { continue; }
            Handles.DrawSolidArc(blockOriginPos, Vector3.forward, Quaternion.Euler(0f, 0f, block.minAngle) * forwardDirection, block.maxAngle - block.minAngle, 1.5f);
        }
        Handles.color = new Color(0.15f, 0.85f, 1f, 0.35f);
        foreach (Types.BlockData block in blockDataAlt)
        {
            if (!block.showDebugVisuals) { continue; }
            Handles.DrawSolidArc(blockOriginPos, Vector3.forward, Quaternion.Euler(0f, 0f, block.minAngle) * forwardDirection, block.maxAngle - block.minAngle, 1.5f);
        }
    }
#endif
}
