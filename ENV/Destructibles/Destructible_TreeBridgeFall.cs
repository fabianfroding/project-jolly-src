using UnityEngine;

public class Destructible_TreeBridgeFall : Destructible
{
    [SerializeField] private Collider2D colliderAlive;
    [SerializeField] private Collider2D colliderDead;
    [SerializeField] private GameObject[] leafSFXPosition;
    [SerializeField] private GameObject[] leafSFX;

    public override void TakeDamage()
    {
        base.TakeDamage();
        // Create leaf particles on damage.
        if (health > 0)
        for (int i = 0; i < leafSFXPosition.Length; i++)
        {
            int numLeaves = Random.Range(1, 3);
            for (int j = 0; j < numLeaves; j++)
            {
                GameObject sfx = Instantiate(leafSFX[Random.Range(0, leafSFX.Length)], leafSFXPosition[i].transform);
                    sfx.GetComponent<ParticleSystem>().Play();
                Destroy(sfx, sfx.GetComponent<ParticleSystem>().main.duration);
            }
        }
    }

    protected override void DestroySelf()
    {
        base.DestroySelf();
        colliderDead.enabled = true;
        colliderAlive.enabled = false;
        tag = EditorConstants.TAG_GROUND;
        gameObject.layer = LayerMask.NameToLayer(EditorConstants.LAYER_GROUND);
    }
}
