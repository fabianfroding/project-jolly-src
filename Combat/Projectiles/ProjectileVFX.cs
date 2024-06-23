using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVFX : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] trailRenderers;
    [SerializeField] private ParticleSystem[] particleSystems;

    private List<float> durations;

    private void Awake()
    {
        durations = new List<float>();
    }

    public void StopEmitting()
    {
        for (int i = 0; i < trailRenderers.Length; i++)
        {
            trailRenderers[i].emitting = false;
            durations.Add(trailRenderers[i].time);
        }
        for (int j = 0; j < particleSystems.Length; j++)
        {
            particleSystems[j].Stop(false);
            durations.Add(particleSystems[j].main.duration);
        }

        float longestDur = Mathf.Max(durations.ToArray());
        StopCoroutine(DestroySelf());
        StartCoroutine(DestroySelf(longestDur + 0.5f));
    }

    private IEnumerator DestroySelf(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
