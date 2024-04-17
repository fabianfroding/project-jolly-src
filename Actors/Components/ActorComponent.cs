using UnityEngine;

public class ActorComponent : MonoBehaviour
{
    protected Actor owningActor;

    #region Unity Callbacks
    protected virtual void Awake()
    {
        gameObject.name = GetType().Name;
    }
    #endregion

    public Actor GetOwningActor()
    {
        if (owningActor)
            return owningActor;
        owningActor = GetComponentInParent<Actor>();
        if (owningActor)
            return owningActor;
        Debug.LogWarning("ActorComponent::GetOwningActor: Could not find owning actor for " + gameObject.name);
        return null;
    }
}
