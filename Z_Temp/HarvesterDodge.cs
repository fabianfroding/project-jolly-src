using UnityEngine;

public class HarvesterDodge : Ability
{
    [SerializeField] private string animName = "Harvester_Dodge";
    //[SerializeField] private float dodgeOffset = 8f;
    [SerializeField] private GameObject dodgeSoundPrefab;

    private Animator animator;
    private GameObject user;

    //==================== PUBLIC ====================//
    public bool StartDodge(GameObject target)
    {
        if (base.StartAbility())
        {
            animator.Play(animName);

            if (dodgeSoundPrefab != null && CameraManager.IsGameObjectInCameraView(gameObject))
            {
                GameObject sound = Instantiate(dodgeSoundPrefab, transform.position, Quaternion.identity);
                Destroy(sound, sound.GetComponent<AudioSource>().clip.length);
            }

            //transform.position = new Vector3(target.transform.position.x + dodgeOffset * GetComponent<Enemy>().Core.Movement.FacingDirection, transform.position.y, transform.position.z);
            //GetComponent<Enemy>().Core.Movement.Flip();

            return true;
        }
        return false;
    }

    //==================== PRIVATE ====================//
    private void Awake()
    {
        animator = GetComponent<Animator>();
        user = gameObject;
    }
}
