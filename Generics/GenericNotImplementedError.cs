using UnityEngine;

public static class GenericNotImplementedError<T>
{
    public static T TryGet(T value, string name)
    {
        if (value != null)
        {
            return value;
        }
        Debug.LogError(typeof(T) + " not implemented on " + name);
        return default;
    }

    public static T[] TryGet(T[] values, string name)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] == null)
            {
                Debug.LogError(typeof(T) + " not implemented on " + name);
                return default;
            }
        }
        return values;
    }
}
