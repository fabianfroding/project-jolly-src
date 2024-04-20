using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    protected Movement movement;
    
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
        if (!blockingEnabled)
            return false;

        float angle = Vector2.SignedAngle(transform.right, 
            source.transform.position - (blockOriginPosition ? blockOriginPosition.transform.position : target.transform.position));
        
        foreach (Types.BlockData block in useAltBlockData ? blockDataAlt : blockData)
        {
            // Check if the angle is within the defined blocking zone.
            if (angle >= block.minAngle && angle <= block.maxAngle)
            {
                OnAttackBlocked?.Invoke();
                Debug.Log(target.name + " blocked attack from " + source.name);
                return true;
            }
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
