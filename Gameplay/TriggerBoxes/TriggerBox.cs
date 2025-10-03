using UnityEngine;

public class TriggerBox : MonoBehaviour
{
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerOnce && hasTriggered) return;

        PlayerCharacter playerCharacter = collision.GetComponent<PlayerCharacter>();
        if (!playerCharacter) return;

        hasTriggered = true;
        TriggerBehavior();
    }

    protected virtual void TriggerBehavior() {}
}
