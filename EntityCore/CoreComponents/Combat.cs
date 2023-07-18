using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject damagedParticles;

    [Header("Knockback Settings")]
    [SerializeField] protected float maxKnockbackTime = 0.2f;
    protected bool isKnockbackActive = false;
    protected float knockbackStartTime;

    [Header("Stun Settings")]
    [Tooltip("Check to allow the entity to recieve stun damage.")]
    [SerializeField] protected bool stunnable = false;
    [Tooltip("The time-window in which an enity can get stunned after it has become vulnerable to stun damage.")]
    [SerializeField] protected float stunTimeWindow = 2f;

    [Header("Parry Settings")]
    [Tooltip("Check to allow an enemy to be parried. This field is for Enemies only. " +
        "Requires setup of appropriate animation parameters TriggerParriable and Trigger Attack, " +
        "which represents the time-window that the enemy can be parried.")]
    [SerializeField] protected bool canBeParried = false;

    [Header("Block Settings")]
    [SerializeField] public bool canBlock = false;
    [SerializeField] public List<Types.BlockData> blockData;
    public GameObject blockOriginPosition;
    public bool blockingEnabled = false;

    // Time-window between the attacking-part of a melee attack animation.
    public bool IsInTriggeredParriedAnimationFrames { get; set; }

    public bool IsStunned { get; protected set; }
    public float LastStunDamageTime { get; protected set; }
    protected float currentStunResistance;

    [Header("Spawned Objects")]
    public GameObject parriedSoundPrefab;
    [SerializeField] protected GameObject takeDmgSFXPrefab;
    [SerializeField] protected GameObject takeDmgSoundPrefab;

    protected Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
    protected CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses);
    protected Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    protected ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);

    protected Stats stats;
    protected CollisionSenses collisionSenses;
    protected Movement movement;
    protected ParticleManager particleManager;

    public event Action OnDamaged;
    public event Action OnAttackBlocked;

    public override void LogicUpdate() { CheckKnockback(); }

    public void TakeDamage(Types.DamageData damageData)
    {
        if (CheckBlock(damageData.source, damageData.target))
        {
            Debug.Log(damageData.target.name + " blocked attack from " + damageData.source.name);
            OnAttackBlocked?.Invoke();
            return;
        }
        
        if (Stats.currentHealth > 0)
        {
            Debug.Log(damageData.target.name + " took " + damageData.damageAmount + " damage from " + damageData.source.name);
            Stats.DecreaseHealth(damageData.damageAmount);
            OnDamaged?.Invoke();

            ParticleManager.StartParticlesWithRandomRotation(damagedParticles);
            InstantiateTakeDamageVisuals(damageData); // TODO: Deprecate this.

            // Not sure but I think this is solely used for players rage bar mechanic. Should be moved elsewhere.
            if (damageData.source.CompareTag(EditorConstants.TAG_PLAYER) && !gameObject.CompareTag(EditorConstants.TAG_PLAYER))
            {
                Stats sourceStats = damageData.source.GetComponentInChildren<Stats>();
                if (sourceStats != null)
                {
                    sourceStats.OnDealtDamage();
                }
            }

            ApplyKnockback(damageData);
        }
    }

    protected void InstantiateTakeDamageVisuals(Types.DamageData damageData)
    {
        if (takeDmgSoundPrefab != null && !damageData.ranged)
        {
            GameObject takeDmgSound = Instantiate(takeDmgSoundPrefab, transform.position, Quaternion.identity);
            Destroy(takeDmgSound, takeDmgSound.GetComponent<AudioSource>() != null ? takeDmgSound.GetComponent<AudioSource>().clip.length : 0f);
        }

        if (takeDmgSFXPrefab != null)
        {
            GameObject takeDmgSFX = Instantiate(takeDmgSFXPrefab);
            takeDmgSFX.transform.position = transform.position;
            takeDmgSFX.transform.Rotate(0, 0, 90);
            Destroy(takeDmgSFX, takeDmgSFX.GetComponent<ParticleSystem>() != null ? takeDmgSFX.GetComponent<ParticleSystem>().main.duration : 0f);
        }
    }

    protected bool IsFlyingEnemy()
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        return enemy != null && enemy.enemyData.isFlying;
    }

    protected void ApplyKnockback(Types.DamageData damageData)
    {
        if (damageData.knockbackStrength <= 0f) { return; }

        if (damageData.source != null)
        {
            DamagingObject damagingObject = damageData.source.GetComponent<DamagingObject>();
            if (damageData.ranged)
            {
                Vector2 dir = TrigonometryUtils.GetDirectionBetweenPositions(damagingObject.transform, transform);
                Knockback(damageData.knockbackAngle, damageData.knockbackStrength, dir.x >= 0 ? 1 : -1);
            }
            else
            {
                Vector2 dir = TrigonometryUtils.GetDirectionBetweenPositions(damageData.source.transform, transform);
                Knockback(damageData.knockbackAngle, damageData.knockbackStrength, dir.x >= 0 ? 1 : -1);
            }
        }
        else
        {
            Vector2 dir = TrigonometryUtils.GetDirectionBetweenPositions(damageData.source.transform, transform);
            Knockback(damageData.knockbackAngle, damageData.knockbackStrength, dir.x >= 0 ? 1 : -1);
        }
    }

    public virtual void Knockback(Vector2 angle, float strength, int direction)
    {
        Movement.SetVelocity(strength, angle, direction);
        Movement.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    protected void CheckKnockback()
    {
        if (isKnockbackActive && 
            (IsFlyingEnemy() || Movement.CurrentVelocity.y < 0.01f) && 
            (CollisionSenses.Ground || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
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

    public void ResetStunResistance()
    {
        IsStunned = false;
        currentStunResistance = Stats.GetStunResistance();
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
        
        foreach (Types.BlockData block in blockData)
        {
            // Check if the angle is within the defined blocking zone.
            if (angle >= block.minAngle && angle <= block.maxAngle) { return true; }
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 forwardDirection = transform.right;
        Vector2 blockOriginPos = blockOriginPosition ? blockOriginPosition.transform.position : transform.position;
        Handles.color = new Color(0.15f, 0.15f, 1f, 0.35f);
        foreach (Types.BlockData block in blockData)
        {
            Handles.DrawSolidArc(blockOriginPos, Vector3.forward, Quaternion.Euler(0f, 0f, block.minAngle) * forwardDirection, block.maxAngle - block.minAngle, 1.5f);
        }
    }
#endif
}
