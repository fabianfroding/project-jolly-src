using UnityEngine;

public class PlayRandomAudioClipFromArray : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (GameFunctionLibrary.IsGameObjectInCameraView(gameObject))
        {
            audioSource.clip = audioClip[Random.Range(0, audioClip.Length)];
            audioSource.Play();
        }
    }
}
