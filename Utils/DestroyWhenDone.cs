using UnityEngine;

public class DestroyWhenDone : MonoBehaviour
{
    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem)
        {
            Destroy(gameObject, particleSystem.main.duration);
        }
        else if (audioSource && audioSource.clip)
        {
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
