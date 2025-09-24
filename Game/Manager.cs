using UnityEngine;

public abstract class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance && Instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate {typeof(T)} destroyed.");
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
}
