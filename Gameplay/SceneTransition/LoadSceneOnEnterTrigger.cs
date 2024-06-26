using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnterTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load.")]
    [SerializeField] private string sceneToLoad;

    // TODO: Hide sprite renderer in game
    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sceneToLoad == null || sceneToLoad == "")
        {
            Debug.Log(gameObject.name + " is missing SceneToLoad.");
            return;
        }

        PlayerPawn player = collision.GetComponent<PlayerPawn>();
        if (player)
        {
            Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
            if (rigidbody2D)
                rigidbody2D.gravityScale = 0f;

            SetSceneTransitionData(player);

            ScreenFade screenFade = FindObjectOfType<ScreenFade>();
            if (screenFade)
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

    private void SetSceneTransitionData(PlayerPawn player)
    {
        if (player != null)
            SceneTransitionDataHolder.playerFacingDirection = player.Core.GetCoreComponent<Movement>().FacingDirection;
        SceneTransitionDataHolder.spawnPointName = SceneManager.GetActiveScene().name;
    }

    private void LoadScene(string sceneToLoad) => SceneManager.LoadScene(sceneToLoad);

    private IEnumerator LoadScene(string sceneToLoad = null, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        LoadScene(sceneToLoad);
    }
}
