using System;
using UnityEngine;

public class LoadSceneSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
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
            SceneTransitionDataHolder.loadSceneSpawnPointName = null;

            ScreenFade screenFade = FindObjectOfType<ScreenFade>();
            if (screenFade != null)
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
        return SceneTransitionDataHolder.loadSceneSpawnPointName != null &&
            name == SceneTransitionDataHolder.loadSceneSpawnPointName;
    }

    private void LoadSceneTransitionData(Player player)
    {
        if (player != null)
        {
            player.Core.GetCoreComponent<Stats>().SetHealth(SceneTransitionDataHolder.playerCurrentHealth);
            player.Core.GetCoreComponent<Stats>().SetMaxHealth(SceneTransitionDataHolder.playerMaxHealth);
            Movement playerMovement = player.Core.GetCoreComponent<Movement>();
            if (playerMovement.FacingDirection != SceneTransitionDataHolder.playerFacingDirection)
            {
                playerMovement.Flip();
            }
        }
    }
}
