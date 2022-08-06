using UnityEngine;

public class NPCMarble : NPC
{
    [SerializeField] private AudioSource doorSlamRef;

    //==================== PRIVATE ====================//
    private void FixedUpdate()
    {
        /*if (UIManager.GetUIManagerScript().GetClockUI() != null)
        {
            if (!ClockManager.IsNightTime() && !spriteRenderer.enabled)
            {
                spriteRenderer.enabled = true;
                PlayDoorSlamSound();
            }
            else if (ClockManager.IsNightTime() && spriteRenderer.enabled)
            {
                spriteRenderer.enabled = false;
                PlayDoorSlamSound();
            }
        }*/
    }

    // Triggered at the end of idle animation.
    private void AnimationBlink()
    {
        /*if (Random.Range(0, 2) == 1)
        {
            Animator.Play("Marble_Idle", 0, 0.0f);
        }*/
    }

    private void PlayDoorSlamSound()
    {
        /*if (CameraManager.IsGameObjectInCameraView(gameObject))
        {
            doorSlamRef.Play();
        }*/
    }
}
