using UnityEngine;

public class EnvironmentInteractLeaves : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystems;
    private AudioSource interactSound;

    private void Awake()
    {
        interactSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }
            interactSound.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }
        }
    }
}
