using System.Collections;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 2f;
    public static Vector3 currentRespawnPoint;
    private Coroutine respawnPlayerCoroutine;

    #region Unity Callback Functions
    private void Awake()
    {
        CombatPlayer.OnPlayerTakeDamageFromENV += RespawnPlayerDelayed;
    }

    private void Start()
    {
        // Called in start to ensure player is already spawned. (Spawned in Awake in LoadSceneSpawnPoint).
        LoadSceneSpawnPoint.OnPlayerEnterScene += SetCurrentRespawnPointToPlayerPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(EditorConstants.TAG_PLAYER))
        {
            currentRespawnPoint = gameObject.transform.position;
        }
    }

    private void OnDisable()
    {
        LoadSceneSpawnPoint.OnPlayerEnterScene -= SetCurrentRespawnPointToPlayerPosition;
    }
    #endregion

    #region Other Functions
    private void RespawnPlayerDelayed()
    {
        if (respawnPlayerCoroutine != null)
        {
            StopCoroutine(respawnPlayerCoroutine);
        }
        respawnPlayerCoroutine = StartCoroutine(RespawnPlayerCR());
    }

    private IEnumerator RespawnPlayerCR()
    {
        yield return new WaitForSeconds(respawnDelay);
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Player player = FindObjectOfType<Player>();
        if (!player.IsDead())
        {
            player.transform.position = currentRespawnPoint;
            player.ResetState(); // Change from dead state.
        }
    }

    /* Called when player is instantiated after a scene transition to make sure current respawn point is not null.
        This ensures that there will always be a current respawn point in the active scene. */
    private void SetCurrentRespawnPointToPlayerPosition() => 
        currentRespawnPoint = FindObjectOfType<Player>().transform.position;
    #endregion
}
