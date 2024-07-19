using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnterTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load.")]
    [SerializeField] private string sceneToLoad;

    private void Start()
    {
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
            Debug.LogError("LoadSceneOnEnterTrigger::OnTriggerEnter2D: Undefined sceneToLoad in " + gameObject.name);
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
