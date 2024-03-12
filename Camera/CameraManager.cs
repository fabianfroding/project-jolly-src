using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static bool IsGameObjectInCameraView(GameObject obj)
    {
        GameObject mainCam = GameObject.FindObjectOfType<CameraScript>().gameObject;
        Vector3 viewPos = mainCam.GetComponent<Camera>().WorldToViewportPoint(obj.transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            return true;
        }
        return false;
    }
}
