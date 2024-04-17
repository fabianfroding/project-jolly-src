using UnityEngine;
using UnityEngine.SceneManagement;

public class WidgetMainMenu : MonoBehaviour
{
    [SerializeField] private string loadedSceneName;

    public void Play()
    {
        SceneManager.LoadScene(loadedSceneName);
    }
}
