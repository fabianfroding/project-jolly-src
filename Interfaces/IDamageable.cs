using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(GameObject source, Damage damage, GameObject damagingObject = null);
}
