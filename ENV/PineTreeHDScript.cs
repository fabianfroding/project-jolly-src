using UnityEngine;

public class PineTreeHDScript : MonoBehaviour
{
    //[SerializeField] private string idleAnim = "PineTreeHD_Idle";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void RandomizeAnimSpeed()
    {
        animator.speed = Random.Range(0.8f, 1.2f);
    }
}
