using UnityEngine;

public class PlayAnimOnEnterTrigger : MonoBehaviour
{
    [SerializeField] private string animNameToPlay;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!animator) return;
        CharacterBase collidingCharacter = collision.GetComponent<CharacterBase>();
        if (!collidingCharacter) return;
        animator.Play(animNameToPlay);
    }
}
