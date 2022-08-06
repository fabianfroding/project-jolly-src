using UnityEngine;

public class UIMap : MonoBehaviour
{
    [SerializeField] private GameObject playerIndicator;

    private void OnEnable()
    {
        UpdatePlayerIndicatorPosition();
    }

    private void UpdatePlayerIndicatorPosition()
    {
        GameObject player = FindPlayer();
        if (player != null)
        {
            playerIndicator.transform.position = new Vector3(
                transform.position.x + player.transform.position.x * 1.25f,
                transform.position.y + player.transform.position.y * 1.25f,
                0);
        }
    }

    private GameObject FindPlayer()
    {
        return FindObjectOfType<Player>().gameObject;
    }
}
