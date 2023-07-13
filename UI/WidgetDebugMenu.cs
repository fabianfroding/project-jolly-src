using UnityEngine;
using UnityEngine.UI;

public class WidgetDebugMenu : MonoBehaviour
{
    [SerializeField] private Button addWorldTokensButton;

    private void Awake()
    {
        if (addWorldTokensButton)
        {
            addWorldTokensButton.onClick.AddListener(AddWorldTokensButtonClicked);
        }
    }

    private void AddWorldTokensButtonClicked()
    {
        PlayerRepository.WorldTokens += 50;
    }
}
