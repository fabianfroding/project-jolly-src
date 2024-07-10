using UnityEngine;

public static class GameFunctionLibrary
{
    public static void InstantiateParticleSystemAtPosition(GameObject particleSystemGO, Vector2 position, bool destroyWhenDone = true)
    {
        // Consider using additional params such as attach to parent for attached vfx, and wrap in struct if too many params.
        if (!particleSystemGO)
        {
            Debug.LogWarning("GameFunctionLibrary::InstantiateParticleSystemAtPosition: Invalid game object!");
            return;
        }
        if (!particleSystemGO.GetComponent<ParticleSystem>())
        {
            Debug.LogWarning("GameFunctionLibrary::InstantiateParticleSystemAtPosition: The game object " +
                particleSystemGO.name + " has no particle system component!");
            return;
        }
        GameObject particleSystemGOInstance = GameObject.Instantiate(particleSystemGO);
        particleSystemGOInstance.transform.parent = null;
        particleSystemGOInstance.transform.position = position;
        if (destroyWhenDone)
            particleSystemGOInstance.AddComponent<DestroyWhenDone>();
    }

    public static bool IsGameObjectInCameraView(GameObject obj)
    {
        GameObject mainCam = GameObject.FindObjectOfType<CameraScript>().gameObject;
        Vector3 viewPos = mainCam.GetComponent<Camera>().WorldToViewportPoint(obj.transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            return true;
        return false;
    }

    public static void PlayAudioAtPosition(AudioClip audioClip, Vector2 position, bool destroyWhenDone = true)
    {
        // TODO: Might want to set spatial blend to 3D.
        // If additional settings are needed, consider defining a serialized struct to avoid excessive parameters.

        if (!audioClip)
            return;
        GameObject soundGO = new("SoundInstance");
        soundGO.transform.parent = null;
        soundGO.transform.position = position;
        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
        if (destroyWhenDone)
            soundGO.AddComponent<DestroyWhenDone>();
    }

    public static void PlayRandomAudioAtPosition(AudioClip[] audioClips, Vector2 position, bool destroyWhenDone = true)
    {
        if (audioClips.Length <= 0)
            return;
        PlayAudioAtPosition(audioClips[Random.Range(0, audioClips.Length - 1)], position, destroyWhenDone);
    }

    #region Math Functions
    public static float GetAngleBetweenObjects(GameObject source, GameObject target)
    {
        return 360f + Mathf.Rad2Deg * Mathf.Atan2(target.transform.position.y - source.transform.position.y, target.transform.position.x - source.transform.position.x);
    }

    public static Vector2 GetDirectionFromAngle(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    // Might come in handy if problems arise with the above version with 1 parameter.
    public static Vector2 GetDirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public static Vector2 GetDirectionBetweenPositions(Transform fromPos, Transform toPos)
    {
        return (toPos.position - fromPos.position).normalized;
    }
    #endregion
}
