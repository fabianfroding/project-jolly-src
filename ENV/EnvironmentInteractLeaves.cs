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
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.CompareTag(EditorConstants.TAG_PLAYER) ||
            enemy ||
            collision.CompareTag(EditorConstants.TAG_NPC))
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
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (collision.CompareTag(EditorConstants.TAG_PLAYER) ||
           enemy ||
            collision.CompareTag(EditorConstants.TAG_NPC))
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }
        }
    }
}
