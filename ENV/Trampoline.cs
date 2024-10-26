using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float trampolineVelocity = 40f;
    [SerializeField] private AudioClip trampolineAudioClip; // TODO: Make this generalized so it doesn't have to be set for each individual trampoline.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            PlayerPawn player = collision.GetComponent<PlayerPawn>();
            if (!player) { return; }

            CollisionSenses collisionSenses = player.GetComponentInChildren<CollisionSenses>();
            if (!collisionSenses) { return; }

            if (collisionSenses.Ground) { return; }

            Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
            if (!rigidbody2D) { return; }

            if (rigidbody2D.linearVelocity.y > 0) { return; }

            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, trampolineVelocity);

            GameFunctionLibrary.PlayAudioAtPosition(trampolineAudioClip, player.transform.position);
        }
    }
}
