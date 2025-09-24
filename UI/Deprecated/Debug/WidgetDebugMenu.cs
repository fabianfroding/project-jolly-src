using UnityEngine;
using UnityEngine.UI;

public class WidgetDebugMenu : MonoBehaviour
{
    [SerializeField] private Button unlockAbilitiesButton;

    private void Awake()
    {
        if (unlockAbilitiesButton) { unlockAbilitiesButton.onClick.AddListener(UnlockAbilitiesButtonClicked); }
    }

    private void UnlockAbilitiesButtonClicked()
    {
        PlayerPawn player = Object.FindFirstObjectByType<PlayerPawn>();
        if (player)
            player.EnableAllLockedStates();
    }
}
