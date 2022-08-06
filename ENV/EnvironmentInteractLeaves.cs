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
        if (collision.CompareTag(EditorConstants.TAG_PLAYER) ||
            collision.CompareTag(EditorConstants.TAG_ENEMY) ||
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
        if (collision.CompareTag(EditorConstants.TAG_PLAYER) ||
            collision.CompareTag(EditorConstants.TAG_ENEMY) ||
            collision.CompareTag(EditorConstants.TAG_NPC))
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }
        }
    }
}
