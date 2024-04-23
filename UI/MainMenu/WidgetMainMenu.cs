using UnityEngine;
using UnityEngine.SceneManagement;

public class WidgetMainMenu : MonoBehaviour
{
    [SerializeField] private string newGameSceneName;

    public void Play()
    {
        string sceneToLoad = newGameSceneName;

        PlayerSaveData playerSaveData = SaveManager.LoadPlayerSaveData();
        if (playerSaveData != null && playerSaveData.sceneName != "")
            sceneToLoad = playerSaveData.sceneName;

        SceneManager.LoadScene(sceneToLoad);
    }

    public void ClearSave()
    {
        SaveManager.ClearPlayerSaveData();
    }
}
