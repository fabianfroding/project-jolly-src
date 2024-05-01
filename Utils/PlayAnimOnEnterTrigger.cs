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
        PawnBase collidingPawn = collision.GetComponent<PawnBase>();
        if (!collidingPawn) return;
        animator.Play(animNameToPlay);
    }
}
