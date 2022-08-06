using UnityEngine;

public class PlayerAbility : Ability
{
    [SerializeField] protected int cost = 0;

    //==================== PUBLIC ====================//
    public override bool StartAbility()
    {
        if (base.StartAbility() /*&& 
            PlayerManager.GetPlayer().GetComponent<PlayerCharacter>().GetEnergy() >= cost*/)
        {
            //PlayerManager.GetPlayer().GetComponent<PlayerCharacter>().SetEnergy(PlayerManager.GetPlayerEnergy() - cost);
            return true;
        }
        return false;
    }
}
