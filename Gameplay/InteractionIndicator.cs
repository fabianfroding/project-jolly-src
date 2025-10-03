 using UnityEngine;

public class InteractionIndicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private IInteractable interactable;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        interactable = GetComponentInParent<IInteractable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerCharacter>(out var playerCharacter))
        {
            if (interactable == null)
            {
                Debug.Log("InteractionIndicator::OnTriggerEnter2D: Failed to get interactable in the indicator's owner.");
                return;
            }
            spriteRenderer.enabled = true;
            playerCharacter.OnEnterInteractionIndicator(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerCharacter>(out var playerCharacter))
        {
            spriteRenderer.enabled = false;
            playerCharacter.OnExitInteractionIndicator();
        }
    }
}
