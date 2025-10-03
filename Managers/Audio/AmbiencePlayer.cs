using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip currentAmbience;

    public void UpdateAmbienceVolume()
    {
        PlayOrStopAmbienceBasedOnVolume(audioSource, audioSource.volume);
    }

    private void PlayOrStopAmbienceBasedOnVolume(AudioSource ambienceAudioSource, float volume)
    {
        if (!ambienceAudioSource) return;
        if (volume <= 0f && ambienceAudioSource.isPlaying)
            ambienceAudioSource.Stop();
        else if (volume > 0f && !ambienceAudioSource.isPlaying)
            ambienceAudioSource.Play();
    }
}
