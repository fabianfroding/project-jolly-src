using UnityEngine;

public class TakeEnvDamageOnCollisionEnter : MonoBehaviour
{
    [SerializeField] private Types.DamageData damageData;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Entity damagedEntity = collision.gameObject.GetComponent<Entity>();
        if (damagedEntity != null)
        {
            damagedEntity.TakeDamage(damageData);
        }
    }
}
