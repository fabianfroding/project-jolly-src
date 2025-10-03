using UnityEngine;
using UnityEngine.SceneManagement;

public class RevivePoint : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject takeDamageVFX;
    [SerializeField] private GameObject takeDamageVFXPosition;
    [SerializeField] private GameObject glowGameObject;
    [SerializeField] private string takeDamageAnimationName;

    private Animator animator;
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(Types.DamageData damageData)
    {
        if (!damageData.source || damageData.ranged)
            return;

        PlayerCharacter playerCharacter = damageData.source.GetComponent<PlayerCharacter>();
        if (!playerCharacter)
            return;

        if (audioSource)
            audioSource.Play();

        if (animator)
            animator.Play(takeDamageAnimationName);

        if (takeDamageVFX)
        {
            GameObject takeDamageVFXInstance = GameObject.Instantiate(takeDamageVFX);
            takeDamageVFXInstance.transform.position = takeDamageVFXPosition.transform.position;
        }

        if (glowGameObject)
            glowGameObject.SetActive(true);
    }
}
