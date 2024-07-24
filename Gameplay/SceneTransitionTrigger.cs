using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load.")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject sceneTransitionSpawnPoint;

    private void Start()
    {
        if (sceneTransitionSpawnPoint)
        {
            PlayerPawn playerPawn = PlayerPawn.Instance;
            if (!playerPawn)
            {
                Debug.LogError(GetType().Name +  "::Start: Failed to find PlayerPawn!");
                return;
            }

            if (playerPawn.currentSceneName != null &&
                playerPawn.currentSceneName != "" &&
                sceneToLoad == playerPawn.currentSceneName)
            {
                playerPawn.transform.position = sceneTransitionSpawnPoint.transform.position;
                playerPawn.currentSceneName = SceneManager.GetActiveScene().name;

                ScreenFade screenFade = FindObjectOfType<ScreenFade>();
                if (screenFade)
                    screenFade.FadeIn();
            }
        }
        else
        {
            Debug.LogError(GetType().Name +  "::Start: No spawn point found for " + gameObject.name);
        }

#if !UNITY_EDITOR
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            spriteRenderer.enabled = false;
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sceneToLoad == null || sceneToLoad == "")
        {
            Debug.LogError(GetType().Name +  "::OnTriggerEnter2D: Undefined sceneToLoad in " + gameObject.name);
            return;
        }

        PlayerPawn player = collision.GetComponent<PlayerPawn>();
        if (player)
        {
            ScreenFade screenFade = FindObjectOfType<ScreenFade>();
            if (screenFade)
            {
                screenFade.FadeOut();
                StartCoroutine(LoadScene(sceneToLoad, screenFade.GetFadeOutDuration()));
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private IEnumerator LoadScene(string sceneToLoad = null, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
