using UnityEngine;

public class PickupableAbility : Pickupable
{
    [SerializeField] private AudioClip abilityUnlockedAudioClip; // TODO: Better have this in SO to avoid having to set it for each unlockable ability.

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPawn player = collision.gameObject.GetComponent<PlayerPawn>();
        if (player)
        {
            UnlockAbility(player);
            GameFunctionLibrary.PlayAudioAtPosition(abilityUnlockedAudioClip, transform.position);
            SaveManager.SavePickupable(GetPickupableUniqueName());
            Destroy(gameObject);
        }
    }

    protected virtual void UnlockAbility(PlayerPawn playerPawn) {}
}
