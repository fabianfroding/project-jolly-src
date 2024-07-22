public class PickupableAbilityWarp : PickupableAbility
{
    protected override void UnlockAbility(PlayerPawn playerPawn)
    {
        base.UnlockAbility(playerPawn);
        playerPawn.EnableWarpState();
    }
}
