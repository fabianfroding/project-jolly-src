using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        MusicPlayer[] musicPlayers = GameObject.FindObjectsByType<MusicPlayer>(FindObjectsSortMode.None);

        if (musicPlayers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
