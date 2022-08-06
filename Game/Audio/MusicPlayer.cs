using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private void Awake()
    {
        MusicPlayer[] musicPlayers = GameObject.FindObjectsOfType<MusicPlayer>();

        if (musicPlayers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
