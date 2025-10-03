using UnityEngine;

public class CoreComponent : MonoBehaviour, ILogicUpdate
{
    protected CharacterCore core;
    protected CharacterBase componentOwner;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<CharacterCore>();
        componentOwner = GetComponentInParent<CharacterBase>();

        if (core == null) { Debug.LogError("There is no Core on the parent."); }
        core.AddComponent(this); // Add the component to the list.
    }

    protected virtual void Start() {}

    public virtual void LogicUpdate() {}
}
