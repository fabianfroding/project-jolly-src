using System;
using System.Collections;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] protected float duration = 5f;
    [SerializeField] protected float effectAmount = 0f;
    public string statusEffectName { get; protected set; }
    protected Entity target;

    public event Action<StatusEffect> OnStatusEffectEnded;

    protected void Awake()
    {
        statusEffectName = transform.name;
    }

    public virtual void Initialize(Entity target)
    {
        this.target = target;
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
