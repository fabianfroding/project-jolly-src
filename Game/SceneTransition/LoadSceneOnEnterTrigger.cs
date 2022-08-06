using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnterTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load.")]
    [SerializeField] private string sceneToLoad;

    [Tooltip("Name of the game object where the player should spawn in the loaded scene.")]
    [SerializeField] private string spawnPointName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sceneToLoad == null || sceneToLoad == "" || spawnPointName == null || spawnPointName == "")
        {
            Debug.Log(gameObject.name + " is missing SceneToLoad or SpawnPointName.");
            return;
        }

        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                SetSceneTransitionData(player);
                player.ToggleLockState();

                ScreenFade screenFade = FindObjectOfType<ScreenFade>();
                if (screenFade != null)
                {
                    screenFade.FadeOut();
                    StopCoroutine(LoadScene());
                    StartCoroutine(LoadScene(sceneToLoad, screenFade.GetFadeOutDuration()));
                }
                else
                {
                    LoadScene(sceneToLoad);
                }
            }
        }
    }

    private void SetSceneTransitionData(Player player)
    {
        if (player != null)
        {
            SceneTransitionDataHolder.playerCurrentHealth = player.Core.GetCoreComponent<Stats>().currentHealth;
            SceneTransitionDataHolder.playerMaxHealth = player.Core.GetCoreComponent<Stats>().GetMaxHealth();
            SceneTransitionDataHolder.playerFacingDirection = player.Core.GetCoreComponent<Movement>().FacingDirection;
        }
        SceneTransitionDataHolder.loadSceneSpawnPointName = spawnPointName;
    }

    private void LoadScene(string sceneToLoad) => SceneManager.LoadScene(sceneToLoad);

    private IEnumerator LoadScene(string sceneToLoad = null, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        LoadScene(sceneToLoad);
    }
}
