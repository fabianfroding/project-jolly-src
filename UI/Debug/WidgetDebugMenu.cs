using System;
using UnityEngine;
using UnityEngine.UI;

public class WidgetDebugMenu : MonoBehaviour
{
    [SerializeField] private Button unlockAbilitiesButton;
    [SerializeField] private Button addWorldTokensButton;

    private void Awake()
    {
        if (unlockAbilitiesButton) { unlockAbilitiesButton.onClick.AddListener(UnlockAbilitiesButtonClicked); }
        if (addWorldTokensButton) { addWorldTokensButton.onClick.AddListener(AddWorldTokensButtonClicked); }
    }

    private void AddWorldTokensButtonClicked()
    {
        PlayerRepository.WorldTokens += 50;
    }

    private void UnlockAbilitiesButtonClicked()
    {
        PlayerAbilityManager playerAbilityManager = FindAnyObjectByType<PlayerAbilityManager>();
        if (playerAbilityManager)
        {
            foreach (PlayerAbilityManager.PlayerAbility ability in Enum.GetValues(typeof(PlayerAbilityManager.PlayerAbility)))
            {
                playerAbilityManager.EnableAbility(ability);
            }
        }
    }
}
