 using UnityEngine;

/* This class is used for detecting when the player comes within range of an NPC to allow dialog.
 * It should be attached to a child game object of the NPC since the layer collision between the player and NPCs are disabled.
 * The game object that has this script should therefore not inherit the NPC layer from the NPC parent game object.
 */

public class InteractionIndicator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            spriteRenderer.enabled = true;
            animator.Play("Show");
            SetInteractableComponentForPlayer(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            if (gameObject.activeSelf)
                animator.Play("Hide");
            SetInteractableComponentForPlayer(null);
        }
    }

    private void SetInteractableComponentForPlayer(Collider2D other)
    {
        if (!other) return;
        if (!other.TryGetComponent(out Player player)) return;

        IInteractable InteractableComponent = GetComponentInParent<IInteractable>();
        if (InteractableComponent == null) return;

        player.currentInteractionTarget = InteractableComponent;
    }

    public bool IsActive() => spriteRenderer.enabled;

    private void AnimationEventHide() => spriteRenderer.enabled = false;
}
