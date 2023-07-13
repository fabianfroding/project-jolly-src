using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<AmbiencePlayer>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static void ChangeAmbience(AudioClip newAmbienceAudioClip)
    {
        AmbiencePlayer ambiencePlayer = FindObjectOfType<AmbiencePlayer>();
        if (!ambiencePlayer) { return; }

        AudioSource audioSource = ambiencePlayer.GetComponent<AudioSource>();
        if (!audioSource) { return; }

        audioSource.Stop();
        audioSource.clip = newAmbienceAudioClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
