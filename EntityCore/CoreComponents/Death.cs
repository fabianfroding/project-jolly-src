using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticles;
    [SerializeField] private GameObject deathSFXPrefab;

    private ParticleManager ParticleManager => particleManager ? particleManager : core.GetCoreComponent(ref particleManager);
    private ParticleManager particleManager;

    private Stats Stats => stats ? stats : core.GetCoreComponent(ref stats);
    private Stats stats;

    private void OnEnable()
    {
        Stats.OnHealthDepleted += Die;
    }

    private void OnDisable()
    {
        Stats.OnHealthDepleted -= Die; 
    }

    public void Die()
    {
        foreach (var particle in deathParticles)
        {
            if (ParticleManager) { ParticleManager.StartParticles(particle); }
        }

        if (deathSFXPrefab)
        {
            GameObject deathSFX = GameObject.Instantiate(deathSFXPrefab);
            deathSFX.transform.position = transform.position;
        }

        core.transform.parent.gameObject.SetActive(false);
    }
}
