using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneSpawnPoint : MonoBehaviour
{
    [Tooltip("The name of the GameObject at which the player will spawn when transitioning from the scene with this name.")]
    [SerializeField] private string fromSceneName;

    private void Start()
    {
        PlayerPawn playerPawn = PlayerPawn.Instance;
        if (!playerPawn)
        {
            Debug.LogError("LoadSceneSpawnPoint:.Start: Failed to find PlayerPawn!");
            return;
        }

        if (playerPawn.currentSceneName != null &&
            playerPawn.currentSceneName != "" &&
            fromSceneName == playerPawn.currentSceneName)
        {
            playerPawn.transform.position = transform.position;
            playerPawn.currentSceneName = SceneManager.GetActiveScene().name;

            ScreenFade screenFade = FindObjectOfType<ScreenFade>();
            if (screenFade)
                screenFade.FadeIn();
        }
    }
}
