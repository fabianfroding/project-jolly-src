using UnityEngine;
using UnityEngine.SceneManagement;

public class WidgetMainMenu : WidgetBase
{
    [SerializeField] private string startGameScene = "GameScene";

    public void HandleStartGame()
    {
        animator.Play(closeAnim.name);
    }

    public void HandleQuitGame()
    {
        Application.Quit();
    }

    public override void CloseAnimationDone()
    {
        SceneManager.LoadScene(startGameScene);
    }
}
