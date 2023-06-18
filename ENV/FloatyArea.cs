using UnityEngine;

public class FloatyArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            Rigidbody2D playerRigidbody2D = collision.GetComponent<Rigidbody2D>();
            if (!playerRigidbody2D) { return; }

            playerRigidbody2D.gravityScale *= 0.5f;
            Time.timeScale = 0.8f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            Rigidbody2D playerRigidbody2D = collision.GetComponent<Rigidbody2D>();
            if (!playerRigidbody2D) { return; }

            Player player = collision.GetComponent<Player>();
            if (!player) { return; }

            playerRigidbody2D.gravityScale = player.GetPlayerStateData().defaultGravityScale;
            Time.timeScale = 1f;
        }
    }
}
