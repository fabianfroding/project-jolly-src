using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuOptionsMenu : MainMenuSubMenu
{
    [SerializeField] private GameObject optionsAudioMenuGO;
    [SerializeField] private GameObject optionsControlsMenuGO;
    [SerializeField] private GameObject optionsVideoMenuGO;

    public void OptionsAudioClicked()
    {
        OpenNewMenuGameObject(optionsAudioMenuGO);
    }

    public void OptionsControlsClicked()
    {
        OpenNewMenuGameObject(optionsControlsMenuGO);
    }

    public void OptionsVideoClicked()
    {
        OpenNewMenuGameObject(optionsVideoMenuGO);
    }
}
