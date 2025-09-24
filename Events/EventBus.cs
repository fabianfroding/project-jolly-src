using System;
using System.Collections.Generic;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> EventListeners = new();
    
    public static void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (!EventListeners.ContainsKey(eventType))
        {
            EventListeners[eventType] = new List<Delegate>();
        }
        EventListeners[eventType].Add(listener);
    }

    public static void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);
        if (EventListeners.ContainsKey(eventType))
        {
            EventListeners[eventType].Remove(listener);
        }
    }

    public static void Publish<T>(T publishedEvent)
    {
        Type eventType = typeof(T);
        if (EventListeners.ContainsKey(eventType))
        {
            foreach (var listener in EventListeners[eventType])
            {
                (listener as Action<T>)?.Invoke(publishedEvent);
            }
        }
    }
    
    public static void Clear()
    {
        EventListeners.Clear();
    }
}
