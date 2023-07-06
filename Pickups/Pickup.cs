using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] protected GameObject onPickupSoundPrefab;
    protected bool pickedUp = false;
    protected SpriteRenderer spriteRenderer;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (PickupRepository.GetHasBeenPickedUp(name))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            GetPickup();
        }
    }
    #endregion

    #region Other Functions
    protected virtual void GetPickup() 
    {
        if (!pickedUp)
        {
            pickedUp = true;
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;

            PickupRepository.SetHasBeenPickedUp(name, true);

            InstantiateOnPickupSound();
            Destroy(gameObject);
        }
    }

    protected void InstantiateOnPickupSound()
    {
        if (onPickupSoundPrefab != null)
        {
            GameObject onPickupSound = Instantiate(onPickupSoundPrefab);
            onPickupSound.transform.position = transform.position;
            onPickupSound.transform.parent = null;
            AudioSource audioSource = onPickupSound.GetComponent<AudioSource>();
            Destroy(onPickupSound, audioSource != null ? audioSource.clip.length : 0f);
        }
    }
    #endregion
}
