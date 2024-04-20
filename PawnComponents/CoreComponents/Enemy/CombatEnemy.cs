using UnityEngine;

public class CombatEnemy : Combat
{
    [SerializeField] private GameObject hurtSFXPrefab;
    [SerializeField] [Range(0f, 1f)] float chanceToPlayHurtSound;

    protected override void InstantiateTakeDamageVisuals()
    {
        base.InstantiateTakeDamageVisuals();

        if (hurtSFXPrefab && Random.Range(0f, 1f) <= chanceToPlayHurtSound)
        {
            GameObject hurtSFX = GameObject.Instantiate(hurtSFXPrefab);
            hurtSFX.transform.position = transform.position;
        }
    }
}
