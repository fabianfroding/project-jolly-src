using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public CameraScript CameraScript { get; protected set; }

    protected virtual void Awake()
    {
        CameraScript = GetComponent<CameraScript>();
    }
}
