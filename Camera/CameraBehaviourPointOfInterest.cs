using UnityEngine;
public class CameraBehaviourPointOfInterest : CameraBehaviour
{
    private CameraBehaviourVerticalLane cameraVertLane;
    private float poiTValue = 0.01f;

    private void Start()
    {
        cameraVertLane = GetComponent<CameraBehaviourVerticalLane>();
    }

    public Vector2 GetCameraDestination() 
    {
        // first find target vector - a point in between the player and the POI.
        // behaviourTValue makes it really close to the POI, but since the player pos changes it still slightly follows the player 
        return cameraVertLane.CalcPlayerPOITarget();
    }

    public float GetPoiTValue() {
        return poiTValue;
    }
}
