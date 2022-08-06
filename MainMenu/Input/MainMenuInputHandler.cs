using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuInputHandler : MonoBehaviour
{
    public static void InvokeOnMainMenuButtonClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            MainMenuButton.currentSelectedButton.GetComponent<MainMenuButton>().OnSelectedMainMenuButtonClick();
        }
    }
}
