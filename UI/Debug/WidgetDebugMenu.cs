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
        Player player = FindObjectOfType<Player>();
        if (player)
            player.EnableAllLockedStates();
    }
}
