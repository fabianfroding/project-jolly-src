using System.Collections;
using UnityEngine;

public class WalkableCloud : MonoBehaviour
{
    [SerializeField] private float durationBeforeBreak = 1.5f;
    [SerializeField] private float resetDuration = 5f;
    [SerializeField] private GameObject breakSFX;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (!boxCollider2D)
        {
            Debug.LogError("WalkableCloud:Start: Failed to get BoxCollider2D component.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer)
        {
            Debug.LogError("WalkableCloud:Start: Failed to get SpriteRenderer component.");
        }
    }

    private void OnDisable()
    {
        StopCoroutine(Break());
        StopCoroutine(Reset());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(EditorConstants.TAG_PLAYER) && spriteRenderer.enabled)
        {
            boxCollider2D.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER) && spriteRenderer.enabled)
        {
            PlayerAbilityManager playerAbilityManager = collision.GetComponent<PlayerAbilityManager>();
            if (!playerAbilityManager) { return; }
            if (!playerAbilityManager.IsAbilityEnabled(PlayerAbilityManager.PlayerAbility.CloudWalk))  { return; }

            Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
            if (!rigidbody2D) { return; }
            if (rigidbody2D.velocity.y >= 0) { return; }

            gameObject.layer = LayerMask.NameToLayer(EditorConstants.LAYER_GROUND);
            boxCollider2D.isTrigger = false;
            StartCoroutine(Break());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER) && spriteRenderer.enabled)
        {
            boxCollider2D.isTrigger = true;
        }
    }

    private IEnumerator Break()
    {
        yield return new WaitForSeconds(durationBeforeBreak);
        spriteRenderer.enabled = false;
        boxCollider2D.enabled = false;

        if (breakSFX)
        {
            GameObject tempGo = GameObject.Instantiate(breakSFX);
            tempGo.transform.position = transform.position;
        }

        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetDuration);
        spriteRenderer.enabled = true;
        boxCollider2D.enabled = true;
        boxCollider2D.isTrigger = true;
    }
}
