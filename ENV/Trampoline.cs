using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float trampolineVelocity = 40f;
    [SerializeField] private GameObject trampolineSFX; // TODO: Make this generalized so it doesn't have to be set for each individual trampoline.

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            Player player = collision.GetComponent<Player>();
            if (!player) { return; }

            CollisionSenses collisionSenses = player.GetComponentInChildren<CollisionSenses>();
            if (!collisionSenses) { return; }

            if (collisionSenses.Ground) { return; }

            Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
            if (!rigidbody2D) { return; }

            if (rigidbody2D.velocity.y > 0) { return; }

            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, trampolineVelocity);

            if (trampolineSFX)
            {
                GameObject tempGO = GameObject.Instantiate(trampolineSFX);
                tempGO.transform.position = player.transform.position;
            }
        }
    }
}
