using UnityEngine;

public class TakeEnvDamageOnCollisionEnter : MonoBehaviour
{
    [SerializeField] private Damage damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Entity damagedEntity = collision.gameObject.GetComponent<Entity>();
        if (damagedEntity != null)
        {
            damagedEntity.TakeDamage(gameObject, damage);
        }
    }
}
