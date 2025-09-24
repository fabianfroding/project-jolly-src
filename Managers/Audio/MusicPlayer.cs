using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        MusicPlayer[] musicPlayers = FindObjectsByType<MusicPlayer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (musicPlayers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
