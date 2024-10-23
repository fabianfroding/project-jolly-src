using System.Collections;
using UnityEngine;

public class PlayerShield : MonoBehaviour, IDamageable
{
    [SerializeField] private float barrierDuration = 1f;
    [SerializeField] private AudioClip onEndAudioClip;
    private HealthComponent healthComponent;

    private void Awake()
    {
        PawnBase pawnOwner = GetComponent<PawnBase>();
        if (pawnOwner)
            healthComponent = pawnOwner.HealthComponent;
    }

    private void OnEnable()
    {
        if (healthComponent)
            healthComponent.SetInvulnerable(true);
        StartCoroutine(EndBarrier());
    }

    private void OnDisable()
    {
        if (healthComponent)
            healthComponent.SetInvulnerable(false);
    }

    public void TakeDamage(Types.DamageData damageData)
    {
        GameFunctionLibrary.PlayAudioAtPosition(onEndAudioClip, transform.position);
        gameObject.SetActive(false);
        Debug.Log("Barrier Take Dmg");
    }

    private IEnumerator EndBarrier()
    {
        yield return new WaitForSeconds(barrierDuration);
        gameObject.SetActive(false);
    }
}
