using UnityEngine;
using UnityEngine.SceneManagement;

/* This class is used for detecting when the player comes within range of an NPC to allow dialog.
 * It should be attached to a child game object of the NPC since the layer collision between the player and NPCs are disabled.
 * The game object that has this script should therefore not inherit the NPC layer from the NPC parent game object.
 */

public class InteractionIndicator : MonoBehaviour
{
    [Tooltip("For Enter-type interaction indicators only. Set the field to the scene to load.")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject fungusFlowChart;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool playerInRange = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (sceneToLoad != "" && sceneToLoad != null)
            {
                /*if (InputManager.InteractPressed() && PlayerManager.IsGrounded())
                {
                    SceneManager.LoadScene(sceneToLoad);
                }*/
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            playerInRange = true;
            spriteRenderer.enabled = true;

            if (fungusFlowChart)
            {
                fungusFlowChart.SetActive(true);
            }

            animator.Play("Show");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            playerInRange = false;

            if (fungusFlowChart)
            {
                fungusFlowChart.SetActive(false);
            }

            if (gameObject.activeSelf)
                animator.Play("Hide");
        }
    }

    public bool IsActive() => spriteRenderer.enabled;

    private void AnimationEventHide() => spriteRenderer.enabled = false;
}
