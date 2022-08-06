using UnityEngine;

public class PlayRandomAudioClipFromArray : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (CameraManager.IsGameObjectInCameraView(gameObject))
        {
            audioSource.clip = audioClip[Random.Range(0, audioClip.Length - 1)];
            audioSource.Play();
        }
    }
}
