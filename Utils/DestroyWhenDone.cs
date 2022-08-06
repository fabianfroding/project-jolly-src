using UnityEngine;

public class DestroyWhenDone : MonoBehaviour
{
    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Destroy(gameObject, particleSystem.main.duration);
        }
        else if (audioSource != null)
        {
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
