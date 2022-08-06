using System.Collections;
using UnityEngine;

public class DetachWhenDone : MonoBehaviour
{
    private ParticleSystem particleSystemComp;
    private Rigidbody2D rb;

    private void Awake()
    {
        particleSystemComp = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        rb = GetComponentInParent<Rigidbody2D>();

        if (particleSystemComp != null)
        {
            StartCoroutine(DetachAfterDelay(particleSystemComp.main.duration));
        }
        else if (audioSource != null)
        {
            StartCoroutine(DetachAfterDelay(audioSource.clip.length));
        }
    }

    private void FixedUpdate()
    {
        if (particleSystemComp != null && rb.velocity.y < 0)
        {
            particleSystemComp.Stop();
        }
    }

    private IEnumerator DetachAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (particleSystemComp != null)
        {
            particleSystemComp.Stop();
        }
        transform.parent = null;
    }
}
