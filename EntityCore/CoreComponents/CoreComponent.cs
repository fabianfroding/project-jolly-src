using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected EntityCore core;
    protected PawnBase componentOwner;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<EntityCore>();
        componentOwner = GetComponentInParent<PawnBase>();

        if (core == null) { Debug.LogError("There is no Core on the parent."); }
        core.AddComponent(this); // Add the component to the list.
    }

    protected virtual void Start() {}

    public virtual void LogicUpdate() {}
}
