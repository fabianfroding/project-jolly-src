using UnityEngine;

public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField] private AudioClip currentDaytimeAmbience;
    [SerializeField] private AudioClip currentNighttimeAmbience;

    private AudioSource cachedAudioSource;

    private float cachedDawnMid;
    private float cachedDuskMid;

    private void Awake()
    {
        cachedAudioSource = GetComponent<AudioSource>();
        DaytimeManager.OnDaytimeTick += UpdateAmbienceVolume;
    }

    private void Start()
    {
        cachedDawnMid = DaytimeManager.Instance.GetDawnMidTime();
        cachedDuskMid = DaytimeManager.Instance.GetDuskMidTime();
    }

    private void OnDestroy()
    {
        DaytimeManager.OnDaytimeTick -= UpdateAmbienceVolume;
    }

    private void UpdateAmbienceVolume(float timeOfDay)
    {
        if (!cachedAudioSource) return;

        float dawnStart = DaytimeManager.Instance.GetDawnStartTime();
        float dawnEnd = DaytimeManager.Instance.GetDawnEndTime();
        float duskStart = DaytimeManager.Instance.GetDuskStartTime();
        float duskEnd = DaytimeManager.Instance.GetDuskEndTime();

        if (timeOfDay >= dawnStart && timeOfDay < cachedDawnMid)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - dawnStart) / (cachedDawnMid - dawnStart));
            cachedAudioSource.volume = Mathf.Lerp(1f, 0.1f, lerpFactor);
        }
        else if (timeOfDay >= cachedDawnMid && timeOfDay < dawnEnd)
        {
            if (cachedAudioSource.clip != currentDaytimeAmbience)
                ChangeAmbience(currentDaytimeAmbience);
            float lerpFactor = Mathf.Clamp01((timeOfDay - cachedDawnMid) / (dawnEnd - cachedDawnMid));
            cachedAudioSource.volume = Mathf.Lerp(0.1f, 1f, lerpFactor);
        }
        else if (timeOfDay >= duskStart && timeOfDay < cachedDuskMid)
        {
            float lerpFactor = Mathf.Clamp01((timeOfDay - duskStart) / (cachedDuskMid - duskStart));
            cachedAudioSource.volume = Mathf.Lerp(1f, 0.1f, lerpFactor);
        }
        else if (timeOfDay >= cachedDuskMid && timeOfDay < duskEnd)
        {
            if (cachedAudioSource.clip != currentNighttimeAmbience)
                ChangeAmbience(currentNighttimeAmbience);
            float lerpFactor = Mathf.Clamp01((timeOfDay - cachedDuskMid) / (duskEnd - cachedDuskMid));
            cachedAudioSource.volume = Mathf.Lerp(0.1f, 1f, lerpFactor);
        }
    }

    public void ChangeAmbience(AudioClip newAmbienceAudioClip)
    {
        if (!cachedAudioSource) return;
        cachedAudioSource.Stop();
        cachedAudioSource.clip = newAmbienceAudioClip;
        cachedAudioSource.loop = true;
        cachedAudioSource.Play();
    }
}
