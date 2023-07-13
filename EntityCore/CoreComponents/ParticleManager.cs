using UnityEngine;

public class ParticleManager : CoreComponent
{
    private Transform particleContainer;

    protected override void Awake()
    {
        base.Awake();
        particleContainer = GameObject.FindGameObjectWithTag(EditorConstants.TAG_PARTICLE_CONTAINER).transform;
    }

    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation)
    {
        return Instantiate(particlePrefab, position, rotation, particleContainer);
    }

    public GameObject StartParticles(GameObject particlePrefab)
    {
        return StartParticles(particlePrefab, transform.position, Quaternion.identity);
    }

    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab)
    {
        return StartParticles(particlePrefab, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }

    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab, Vector2 position)
    {
        return StartParticles(particlePrefab, position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }
}
