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
        PlayerCharacter player = Object.FindFirstObjectByType<PlayerCharacter>();
        if (player)
            player.EnableAllLockedStates();
    }
}
