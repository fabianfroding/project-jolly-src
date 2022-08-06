using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected EntityCore core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<EntityCore>();

        if (core == null) { Debug.LogError("There is no Core on the parent."); }
        core.AddComponent(this); // Add the component to the list.
    }

    public virtual void LogicUpdate() {}
}
