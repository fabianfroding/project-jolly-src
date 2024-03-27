using UnityEngine;

public class EnvironmentInteract : MonoBehaviour
{
    private static readonly string INTERACT_LEFT_ANIM_NAME = "InteractLeft";
    private static readonly string INTERACT_RIGHT_ANIM_NAME = "InteractRight";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Destructible>() != null && GetComponent<Destructible>().GetHealth() > 0)
        {
            if (collision.gameObject.GetComponent<Entity>())
            {
                if (collision.GetComponent<Entity>().Core.GetCoreComponent<Movement>().FacingDirection == 1)
                    animator.Play(gameObject.name + "_" + INTERACT_LEFT_ANIM_NAME);
                else
                    animator.Play(gameObject.name + "_" + INTERACT_RIGHT_ANIM_NAME);
            }
        }
    }
}
