using System;
using System.Collections;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected float effectAmount = 0f;
    public string StatusEffectName { get; protected set; }
    protected Combat owningCombatComponent;

    public event Action<StatusEffect> OnStatusEffectEnded;

    protected void Awake()
    {
        StatusEffectName = transform.name;
    }

    public virtual void Initialize(Combat targetedCombatComponent)
    {
        this.owningCombatComponent = targetedCombatComponent;
        StartCoroutine(StartDuration());
    }

    protected IEnumerator StartDuration()
    {
        yield return new WaitForSeconds(duration);
        StatusEffectEnded();
    }

    protected virtual void StatusEffectEnded()
    {
        OnStatusEffectEnded?.Invoke(this);
        GameObject.Destroy(gameObject);
    }
}
