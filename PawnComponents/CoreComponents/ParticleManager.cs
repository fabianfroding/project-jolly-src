using UnityEngine;

public class ParticleManager : CoreComponent
{
    private Transform particleContainer;

    protected override void Awake()
    {
        base.Awake();
        GameObject particleContainerGO = GameObject.FindGameObjectWithTag(EditorConstants.TAG_PARTICLE_CONTAINER);
        if (particleContainerGO)
            particleContainer = particleContainerGO.transform;
        else
            Debug.LogWarning("ParticleManager:Awake: Could not find particle container with tag " + EditorConstants.TAG_PARTICLE_CONTAINER + " on " + gameObject.name + ".");
    }

    public GameObject StartParticles(GameObject particlePrefab, Vector2 position, Quaternion rotation)
    {
        if (particleContainer)
            return Instantiate(particlePrefab, position, rotation, particleContainer);
        else
            Debug.LogWarning("ParticleManager:StartParticles: Invalid particle container.");
        return null;
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
