using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource daytimeAudioSource;
    [SerializeField] private AudioSource nighttimeAudioSource;
    [SerializeField] private AudioClip currentDaytimeAmbience;
    [SerializeField] private AudioClip currentNighttimeAmbience;

    [SerializeField] SOIntVariable currentHour;
    [SerializeField] SOIntVariable currentMinute;

    private float cachedDawnMid;
    private float cachedDuskMid;
    private float duskMidOffset;

    private void Start()
    {
        cachedDawnMid = DaytimeManager.Instance.GetDawnMidTime();
        cachedDuskMid = DaytimeManager.Instance.GetDuskMidTime();
        duskMidOffset = cachedDuskMid + (DaytimeManager.Instance.GetDuskEndTime() - DaytimeManager.Instance.GetDuskStartTime());
    }

    public void UpdateAmbienceVolume()
    {
        if (!daytimeAudioSource || !nighttimeAudioSource) return;

        float timeOfDay = currentHour.Value + (currentMinute.Value / 60f);

        float dawnStart = DaytimeManager.Instance.GetDawnStartTime();

        if (timeOfDay >= dawnStart && timeOfDay < cachedDawnMid)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - dawnStart) / (cachedDawnMid - dawnStart));
            daytimeAudioSource.volume = Mathf.Lerp(0f, 1f, lerpFactor);
            nighttimeAudioSource.volume = Mathf.Lerp(1f, 0f, lerpFactor);
        }
        else if (timeOfDay >= cachedDuskMid && timeOfDay < duskMidOffset)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - cachedDuskMid) / (duskMidOffset - cachedDuskMid));
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
