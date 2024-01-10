using System;
using UnityEngine;

public class LoadSceneSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [Tooltip("The point at which the player will spawn when transitioning from the scene with this name.")]
    [SerializeField] private string fromSceneName;

    public static event Action OnPlayerEnterScene;
    private GameObject newPlayer;

    private void Awake()
    {
        if (ShouldLoad())
        {
            DestroyPreexistingPlayers();
            newPlayer = Instantiate(playerPrefab);
            newPlayer.transform.position = transform.position;
            OnPlayerEnterScene?.Invoke();
        }
    }

    private void Start()
    {
        if (ShouldLoad())
        {
            // Load player data in Start to ensure it overrides potential values set in Awake in Stats.
            LoadSceneTransitionData(newPlayer.GetComponent<Player>());
            SceneTransitionDataHolder.spawnPointName = null;

            ScreenFade screenFade = FindObjectOfType<ScreenFade>();
            if (screenFade)
            {
                screenFade.FadeIn();
            }
        }
    }

    private void DestroyPreexistingPlayers()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag(EditorConstants.TAG_PLAYER))
        {
            Destroy(player);
        }
    }

    private bool ShouldLoad()
    {
        return SceneTransitionDataHolder.spawnPointName != null && 
            SceneTransitionDataHolder.spawnPointName != "" &&
            fromSceneName == SceneTransitionDataHolder.spawnPointName;
    }

    private void LoadSceneTransitionData(Player player)
    {
        if (player)
        {
            player.Core.GetCoreComponent<Stats>().SetHealth(SceneTransitionDataHolder.playerHealth);
            Movement playerMovement = player.Core.GetCoreComponent<Movement>();
            if (playerMovement.FacingDirection != SceneTransitionDataHolder.playerFacingDirection)
            {
                playerMovement.Flip();
            }
        }
    }
}
