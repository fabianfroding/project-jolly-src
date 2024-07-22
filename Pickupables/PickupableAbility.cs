using UnityEngine;

public class PickupableAbility : Pickupable
{
    [SerializeField] private AudioClip abilityUnlockedAudioClip;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPawn player = collision.gameObject.GetComponent<PlayerPawn>();
        if (player)
        {
            UnlockAbility(player);
            GameFunctionLibrary.PlayAudioAtPosition(abilityUnlockedAudioClip, transform.position);
            Destroy(gameObject);
        }
    }

    protected virtual void UnlockAbility(PlayerPawn playerPawn) {}
}
