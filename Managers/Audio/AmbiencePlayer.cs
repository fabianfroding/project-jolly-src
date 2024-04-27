using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource daytimeAudioSource;
    [SerializeField] private AudioSource nighttimeAudioSource;
    [SerializeField] private AudioClip currentDaytimeAmbience;
    [SerializeField] private AudioClip currentNighttimeAmbience;

    [Header("Daytime Settings")]
    [SerializeField] private SODaytimeSettings daytimeSettings;

    public void UpdateAmbienceVolume()
    {
        if (!daytimeAudioSource || !nighttimeAudioSource) return;
        if (!daytimeSettings) return;

        float timeOfDay = daytimeSettings.currentHour + (daytimeSettings.currentMinute / 60f);
        float dawnMidTime = daytimeSettings.DawnMidTime; // TODO: Can be cached for performance.
        float duskMidTime = daytimeSettings.DuskMidTime;
        float duskMidOffset = duskMidTime + (daytimeSettings.DuskEndTime - daytimeSettings.DuskStartTime);

        if (timeOfDay >= daytimeSettings.DawnStartTime && timeOfDay < dawnMidTime)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - daytimeSettings.DawnStartTime) / (dawnMidTime - daytimeSettings.DawnStartTime));
            daytimeAudioSource.volume = Mathf.Lerp(0f, 1f, lerpFactor);
            nighttimeAudioSource.volume = Mathf.Lerp(1f, 0f, lerpFactor);
        }
        else if (timeOfDay >= duskMidTime && timeOfDay < duskMidOffset)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - duskMidTime) / (duskMidOffset - duskMidTime));
            daytimeAudioSource.volume = Mathf.Lerp(1f, 0f, lerpFactor);
            nighttimeAudioSource.volume = Mathf.Lerp(0f, 1f, lerpFactor);
        }
        PlayOrStopAmbienceBasedOnVolume(daytimeAudioSource, daytimeAudioSource.volume);
        PlayOrStopAmbienceBasedOnVolume(nighttimeAudioSource, nighttimeAudioSource.volume);
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
