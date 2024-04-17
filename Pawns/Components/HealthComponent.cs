using UnityEngine;

public class HealthComponent : ActorComponent
{
    [Range(0, 999)]
    [SerializeField] protected int currentHealth = 1;
    [Range(1, 999)]
    [SerializeField] protected int maxHealth = 1;

    // Can be turned into a Dictionary if multiple invulernability sources are used.
    [SerializeField] protected bool invulnerable = false;

    #region Unity Callback
    protected virtual void Start()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    #endregion

    public int GetHealth() => currentHealth;
    public void SetHealth(int newHealth) => currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);

    public bool IsAlive() => currentHealth > 0;
}
