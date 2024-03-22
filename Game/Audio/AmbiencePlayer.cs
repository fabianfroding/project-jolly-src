using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField] private AudioClip currentDaytimeAmbience;
    [SerializeField] private AudioClip currentNighttimeAmbience;

    private AudioSource cachedAudioSource;

    private void Awake()
    {
        cachedAudioSource = GetComponent<AudioSource>();
        DaytimeManager.OnHourChange += OnDaytimeHourChanged;
    }

    private void OnDestroy()
    {
        DaytimeManager.OnHourChange -= OnDaytimeHourChanged;
    }

    private void OnDaytimeHourChanged(int hour)
    {
        if (hour == 6)
            ChangeAmbience(currentDaytimeAmbience);
        else if (hour == 18)
            ChangeAmbience(currentNighttimeAmbience);
    }

    public void ChangeAmbience(AudioClip newAmbienceAudioClip)
    {
        if (!cachedAudioSource) { return; }
        cachedAudioSource.Stop();
        cachedAudioSource.clip = newAmbienceAudioClip;
        cachedAudioSource.loop = true;
        cachedAudioSource.Play();
    }
}
