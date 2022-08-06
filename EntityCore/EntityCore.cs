using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityCore : MonoBehaviour
{
    public readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();

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
        var comp = CoreComponents
            .OfType<T>()
            .FirstOrDefault();

        // Check if component was found. Log warning if not.
        if (comp == null)
        {
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        }

        // Return the component.
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
