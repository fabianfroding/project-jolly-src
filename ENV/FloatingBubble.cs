using UnityEngine;

public class FloatingBubble : MonoBehaviour
{
    [SerializeField] private AudioClip deathAudioClip;
    private Rigidbody2D bubbleRigidbody2D;
    private GameObject enteringGameObject; 

    private void Start()
    {
        bubbleRigidbody2D = GetComponent<Rigidbody2D>();

        if (GameFunctionLibrary.IsGameObjectInCameraView(gameObject))
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource)
                audioSource.Play();
        }
    }

    private void OnDestroy()
    {
        if (enteringGameObject)
        {
            PlayerPawn player = enteringGameObject.GetComponent<PlayerPawn>();
            if (player)
                player.StateMachine.ChangeState(player.InAirState);
        }
        GameFunctionLibrary.PlayAudioAtPosition(deathAudioClip, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetFloatingBubbleSettings(collision, true);
        enteringGameObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetFloatingBubbleSettings(collision, false);
        enteringGameObject = null;
    }

    private void SetFloatingBubbleSettings(Collider2D collision, bool entering)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            PlayerPawn player = collision.GetComponent<PlayerPawn>();
            if (player)
            {
                if (entering)
                {
                    player.StateMachine.ChangeState(player.FloatingBubbleState);

                    Rigidbody2D playerRigidbody2D = collision.GetComponent<Rigidbody2D>();
                    if (!playerRigidbody2D)
                    {
                        Debug.LogError("FloatingBubble:SetFloatingBubbleSettings: Failed to get rigid body on collision object.");
                        return;
                    }
                    playerRigidbody2D.velocity = bubbleRigidbody2D.velocity;
                }
                else
                {
                    player.StateMachine.ChangeState(player.InAirState);
                }
            }
        }
    }
}
