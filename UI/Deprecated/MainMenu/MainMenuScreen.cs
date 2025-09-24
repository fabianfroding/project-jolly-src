using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    [SerializeField] private string newGameSceneName;

    public void Play()
    {
        string sceneToLoad = newGameSceneName;

        if (SaveManager.DoesPlayerSaveDataExist())
        {
            PlayerSaveData playerSaveData = SaveManager.LoadPlayerSaveData();
                sceneToLoad = playerSaveData.sceneName;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    public void ClearSave()
    {
        SaveManager.ClearPlayerSaveData();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
