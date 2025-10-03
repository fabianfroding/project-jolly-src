using UnityEngine;

public class RespawnTriggerBox : TriggerBox
{
    [SerializeField] private GameObject fixedRespawnPosition;

    protected override void TriggerBehavior()
    {
        // TODO: Move logic here. Need to pass collision ref.
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
        if (playerCharacter)
        {
            if (fixedRespawnPosition)
                playerCharacter.SetPlayerRespawnPosition(fixedRespawnPosition.transform.position);
            else
                playerCharacter.SetPlayerRespawnPosition(playerCharacter.transform.position);
        }
    }
}
