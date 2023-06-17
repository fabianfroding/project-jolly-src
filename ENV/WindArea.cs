using UnityEngine;

public class WindArea : MonoBehaviour
{
    [SerializeField] private float windVelocity = 8f;
    private Player player;
    private Rigidbody2D playerRigidbody2D;

    private void OnEnable()
    {
        Player.OnPlayerEnterAirGlideState += SetWindVelocity;
        Player.OnPlayerExitAirGlideState += ResetWindVelocity;
    }

    private void OnDisable()
    {
        Player.OnPlayerEnterAirGlideState -= SetWindVelocity;
        Player.OnPlayerExitAirGlideState -= ResetWindVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.LAYER_PLAYER))
        {
            player = collision.GetComponent<Player>();
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
            Player playerComponent = collision.GetComponent<Player>();
            if (playerComponent)
            {
                Rigidbody2D rigidBody2D = playerComponent.GetComponent<Rigidbody2D>();

                if (playerComponent.StateMachine.CurrentState != playerComponent.AirGlideState)
                {
                    rigidBody2D.gravityScale = playerComponent.GetPlayerStateData().defaultGravityScale;
                }

                rigidBody2D.velocity = new Vector3(rigidBody2D.velocity.x, playerComponent.GetPlayerStateData().airGlideFallVelocity, 0);
            }
            player = null;
        }
    }

    private void SetWindVelocity()
    {
        if (player && playerRigidbody2D && player.StateMachine.CurrentState == player.AirGlideState)
        {
            playerRigidbody2D.gravityScale = 0f;
            playerRigidbody2D.velocity = new Vector3(0, windVelocity, 0);
        }
    }

    private void ResetWindVelocity()
    {
        if (player && playerRigidbody2D)
        {
            playerRigidbody2D.gravityScale = player.GetPlayerStateData().defaultGravityScale;
            playerRigidbody2D.velocity = Vector3.zero;
        }
    }
}
