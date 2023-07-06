using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    public enum PlayerAbility
    {
        AirGlide,
        Bow,
        CloudWalk,
        Dash,
        DoubleJump,
        Thunder,
        Warp
    }

    public List<string> enabledAbilities;

    private void Start()
    {
        if (enabledAbilities.Count == 0)
        {
            enabledAbilities = PlayerRepository.GetEnabledAbilities();
            if (enabledAbilities == null)
            {
                enabledAbilities = new List<string>();
            }
        }
    }

    public void EnableAbility(PlayerAbility playerAbility)
    {
        enabledAbilities.Add(playerAbility.ToString());
        PlayerRepository.SaveEnabledAbilities(enabledAbilities);
    }

    public void DisableAbility(PlayerAbility playerAbility)
    {
        enabledAbilities.Remove(playerAbility.ToString());
        PlayerRepository.SaveEnabledAbilities(enabledAbilities);
    }

    public bool IsAbilityEnabled(PlayerAbility playerAbility)
    {
        return enabledAbilities.Contains(playerAbility.ToString());
    }
}
