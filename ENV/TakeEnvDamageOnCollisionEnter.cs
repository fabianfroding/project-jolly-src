using UnityEngine;

public class TakeEnvDamageOnCollisionEnter : MonoBehaviour
{
    [SerializeField] private Types.DamageData damageData;

    private void Awake()
    {
        damageData.source = gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PawnBase pawnBase = collision.gameObject.GetComponent<PawnBase>();
        if (!pawnBase)
            return;

        HealthComponent healthComponent = pawnBase.HealthComponent;
        if (!healthComponent)
            return;

        damageData.target = pawnBase.gameObject;

        EnemyPawn enemyPawn = (EnemyPawn)pawnBase;
        if (enemyPawn)
            healthComponent.Kill();
        else
            healthComponent.TakeDamage(damageData);
    }
}
