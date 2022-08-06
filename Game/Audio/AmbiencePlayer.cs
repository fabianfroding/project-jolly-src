using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    private void Awake()
    {
        AmbiencePlayer[] ambiencePlayers = GameObject.FindObjectsOfType<AmbiencePlayer>();

        if (ambiencePlayers.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
