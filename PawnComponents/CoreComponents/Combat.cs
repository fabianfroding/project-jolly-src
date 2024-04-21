using System;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent
{
    [Header("Stun Settings")]
    [Tooltip("Check to allow the pawn to recieve stun damage.")]
    [SerializeField] protected bool stunnable = false;
    [Tooltip("The time-window in which a pawn can get stunned after it has become vulnerable to stun damage.")]
    [SerializeField] protected float stunTimeWindow = 2f;
    [Tooltip("The amount of hits/damage before the pawn can get stunned.")]
    [SerializeField] protected int stunResistance = 3;

    [Header("Parry Settings")]
    [Tooltip("Check to allow an enemy to be parried. This field is for Enemies only. " +
        "Requires setup of appropriate animation parameters TriggerParriable and Trigger Attack, " +
        "which represents the time-window that the enemy can be parried.")]
    [SerializeField] protected bool canBeParried = false;

    [Header("Block Settings")]
    [Tooltip("If the pawn can block, this is the state it will default to when blocking.")]
    public Types.EBlockState defaultBlockState = Types.EBlockState.E_BlockNone;
    [HideInInspector] public Types.EBlockState blockState = Types.EBlockState.E_BlockNone;

    // Time-window between the attacking-part of a melee attack animation.
    public bool IsInTriggeredParriedAnimationFrames { get; set; }

    public bool IsStunned { get; protected set; }
    public float LastStunDamageTime { get; protected set; }
    protected float currentStunResistance;

    [Header("Spawned Objects")]
    public GameObject parriedSoundPrefab;

    public Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    private Movement movement;
    
    private List<StatusEffect> statusEffects;

    public event Action OnAttackBlocked;

    protected override void Awake()
    {
        base.Awake();
        statusEffects = new List<StatusEffect>();
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

    public bool CheckBlock(GameObject source, GameObject target)
    {
        Debug.Log(target.name + " attempt to block attack from " + source.name);
        if (source == target)
            return false;

        Vector2 attackDirection = (source.transform.position - transform.position).normalized;
        float dotProduct;
        switch (blockState)
        {
            case Types.EBlockState.E_BlockNone:
                return false;

            case Types.EBlockState.E_BlockAll:
                OnAttackBlocked?.Invoke();
                return true;

            case Types.EBlockState.E_BlockFront:
                dotProduct = Vector2.Dot(attackDirection, transform.right);
                if (dotProduct > 0)
                {
                    OnAttackBlocked?.Invoke();
                    return true;
                }
                break;

            case Types.EBlockState.E_BlockAbove:
                dotProduct = Vector2.Dot(attackDirection, transform.up);
                if (dotProduct > 0)
                {
                    OnAttackBlocked?.Invoke(); 
                    return true;
                }
                break;

            default:
                return false;
        }

        return false;
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
}
