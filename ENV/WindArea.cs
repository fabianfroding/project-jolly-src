using UnityEngine;

public class WindArea : MonoBehaviour
{
    [SerializeField] private float windVelocity = 8f;
    private PlayerPawn player;
    private Rigidbody2D playerRigidbody2D;

    private void OnEnable()
    {
        PlayerPawn.OnPlayerEnterAirGlideState += SetWindVelocity;
        PlayerPawn.OnPlayerExitAirGlideState += ResetWindVelocity;
    }

    private void OnDisable()
    {
        PlayerPawn.OnPlayerEnterAirGlideState -= SetWindVelocity;
        PlayerPawn.OnPlayerExitAirGlideState -= ResetWindVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.LAYER_PLAYER))
        {
            player = collision.GetComponent<PlayerPawn>();
            playerRigidbody2D = player.GetComponent<Rigidbody2D>();
            if (player)
            {
                SetWindVelocity();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.LAYER_PLAYER))
        {
            PlayerPawn playerComponent = collision.GetComponent<PlayerPawn>();
            if (playerComponent)
            {
                Rigidbody2D rigidBody2D = playerComponent.GetComponent<Rigidbody2D>();

                if (playerComponent.StateMachine.CurrentState != playerComponent.AirGlideState)
                {
                    rigidBody2D.gravityScale = playerComponent.GetPlayerStateData().defaultGravityScale;
                }
                else
                {
                    rigidBody2D.linearVelocity = new Vector3(rigidBody2D.linearVelocity.x, playerComponent.GetPlayerStateData().airGlideFallVelocity, 0);
                }
            }
            player = null;
        }
    }

    private void SetWindVelocity()
    {
        if (player && playerRigidbody2D && player.StateMachine.CurrentState == player.AirGlideState)
        {
            playerRigidbody2D.gravityScale = 0f;
            playerRigidbody2D.linearVelocity = new Vector3(0, windVelocity, 0);
        }
    }

    private void ResetWindVelocity()
    {
        if (player && playerRigidbody2D)
        {
            playerRigidbody2D.gravityScale = player.GetPlayerStateData().defaultGravityScale;
            playerRigidbody2D.linearVelocity = Vector3.zero;
        }
    }
}
