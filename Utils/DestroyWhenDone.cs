using UnityEngine;

public class DestroyWhenDone : MonoBehaviour
{
    [Tooltip("In case there are no components to base the objects lifetime on, this value will be used instead.")]
    [SerializeField] private float timedLife = 5f;

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
        else
        {
            Destroy(gameObject, timedLife);
        }
    }
}
