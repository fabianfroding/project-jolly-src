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
        Player player = FindObjectOfType<Player>();
        if (player)
            player.EnableAllLockedStates();
    }
}
