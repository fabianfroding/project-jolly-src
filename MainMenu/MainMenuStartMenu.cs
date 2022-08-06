using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuStartMenu : MainMenuSubMenu
{
    [SerializeField] private GameObject profileMenuGO;
    [SerializeField] private GameObject optionsMenuGO;

    public void StartGameClicked()
    {
        OpenNewMenuGameObject(profileMenuGO);
    }

    public void OptionsClicked()
    {
        OpenNewMenuGameObject(optionsMenuGO);
    }

    public void QuitGameClicked()
    {
        Application.Quit();
    }
}
