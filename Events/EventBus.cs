using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> eventListeners = new();
    
    public static void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<Delegate>();
        }
        eventListeners[eventType].Add(listener);
    }

    public static void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType].Remove(listener);
        }
    }

    public static void Publish<T>(T publishedEvent)
    {
        Type eventType = typeof(T);
        if (eventListeners.ContainsKey(eventType))
        {
            foreach (var listener in eventListeners[eventType])
            {
                (listener as Action<T>)?.Invoke(publishedEvent);
            }
        }
    }
}
