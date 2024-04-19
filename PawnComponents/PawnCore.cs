using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PawnCore : MonoBehaviour
{
    public readonly List<CoreComponent> CoreComponents = new();

    public void LogicUpdate()
    {
        // In here we replace calling each LogicUpdate() individually.
        foreach (CoreComponent component in CoreComponents)
        {
            component.LogicUpdate();
        }
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        // Search list for the desired component.
        var comp = CoreComponents.OfType<T>().FirstOrDefault();
        if (comp) { return comp; }

        // Potential sequencing issue fix.
        // Death OnEnable might be called before Stats Awake, causing the event subscribtion to fail and produce an error.
        comp = GetComponentInChildren<T>();
        if (comp) { return comp; }

        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        return comp;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }

    // This function is called by CoreComponents when they want to add themselves to the update list.
    public void AddComponent(CoreComponent component)
    {
        // Check to make sure components is not already part of the list - .Contains() comes from the Linq library we added.
        if (!CoreComponents.Contains(component))
        {
            CoreComponents.Add(component);
        }
    }
}
