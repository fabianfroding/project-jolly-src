using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
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

    public void EnableAbility(string abilityName)
    {
        enabledAbilities.Add(abilityName);
        PlayerRepository.SaveEnabledAbilities(enabledAbilities);
    }

    public void DisableAbility(string abilityName)
    {
        enabledAbilities.Remove(abilityName);
        PlayerRepository.SaveEnabledAbilities(enabledAbilities);
    }

    public bool IsAbilityEnabled(string abilityName)
    {
        return enabledAbilities.Contains(abilityName);
    }
}
