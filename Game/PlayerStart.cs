using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    private static Vector2 location;
    
    private void Awake()
    {
        location = gameObject.transform.position;
    }

    public static void InitializePlayerLocation(GameObject player)
    {
        player.transform.position = location;
    }
    
}
