using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnterTrigger : MonoBehaviour
{
    [Tooltip("Name of the scene to load.")]
    [SerializeField] private string sceneToLoad;

    [SerializeField] private WorldData worldData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (sceneToLoad == null || sceneToLoad == "")
        {
            Debug.Log(gameObject.name + " is missing SceneToLoad.");
            return;
        }

        if (worldData && worldData.world != Types.World.NONE)
        {
            if (PlayerRepository.WorldTokens >= worldData.numWorldTokensRequired)
            {
                List<string> unlockedWorlds = PlayerRepository.GetUnlockedWorlds();
                if (!unlockedWorlds.Contains(worldData.world.ToString()))
                {
                    unlockedWorlds.Add(worldData.world.ToString());
                    PlayerRepository.SaveUnlockedWorlds(unlockedWorlds);
                    PlayerRepository.WorldTokens -= worldData.numWorldTokensRequired;
                    Debug.Log("LoadSceneOnEnterTrigger:OnTriggerEnter2D: " +
                        "Unlocked " + worldData.world.ToString() + ". World Tokens: " + PlayerRepository.WorldTokens);
                }
            }
            else
            {
                Debug.Log("LoadSceneOnEnterTrigger:OnTriggerEnter2D: Not enough world tokens to enter this world!");
                return;
            }
        }

        if (worldData && worldData.ambienceAudioClip)
        {
            AmbiencePlayer.ChangeAmbience(worldData.ambienceAudioClip);
        }

        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            Player player = collision.GetComponent<Player>();
            if (player)
            {
                Rigidbody2D rigidbody2D = collision.GetComponent<Rigidbody2D>();
                if (rigidbody2D)
                {
                    rigidbody2D.gravityScale = 0f;
                }

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
        SceneTransitionDataHolder.spawnPointName = SceneManager.GetActiveScene().name;
    }

    private void LoadScene(string sceneToLoad) => SceneManager.LoadScene(sceneToLoad);

    private IEnumerator LoadScene(string sceneToLoad = null, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        LoadScene(sceneToLoad);
    }
}
