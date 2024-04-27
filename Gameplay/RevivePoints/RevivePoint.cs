using UnityEngine;
using UnityEngine.SceneManagement;

public class RevivePoint : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject takeDamageVFX;
    [SerializeField] private GameObject takeDamageVFXPosition;
    [SerializeField] private GameObject glowGameObject;
    [SerializeField] private string takeDamageAnimationName;
    [SerializeField] private SODaytimeSettings daytimeSettings;

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

        PlayerPawn playerPawn = damageData.source.GetComponent<PlayerPawn>();
        if (!playerPawn)
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

        SaveManager.SavePlayerSaveData(playerPawn, transform.position, SceneManager.GetActiveScene().name, daytimeSettings);
    }
}
