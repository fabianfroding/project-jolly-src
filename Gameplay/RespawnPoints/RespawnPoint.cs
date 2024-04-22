using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject fixedRespawnPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPawn playerPawn = other.GetComponent<PlayerPawn>();
        if (playerPawn)
        {
            if (fixedRespawnPosition)
                playerPawn.SetPlayerRespawnPosition(fixedRespawnPosition.transform.position);
            else
                playerPawn.SetPlayerRespawnPosition(playerPawn.transform.position);
        }
    }
}
