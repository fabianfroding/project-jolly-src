using UnityEngine;

public class PickupableWorldToken : Pickup
{
    protected override void GetPickup()
    {
        if (!pickedUp)
        {
            PlayerRepository.WorldTokens++;
            Debug.Log("PickupableWorldToken:GetPickup: World Tokens: " + PlayerRepository.WorldTokens);
        }
        base.GetPickup();
    }
}
