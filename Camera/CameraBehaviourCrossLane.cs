using UnityEngine;

public class CameraBehaviourCrossLane : CameraBehaviour
{
    private CameraBehaviourVerticalLane cameraVertLane;
    private CameraBehaviourHorizontalLane cameraHoriLane;

    private void Start()
    {
        cameraVertLane = GetComponent<CameraBehaviourVerticalLane>();
        cameraHoriLane = GetComponent<CameraBehaviourHorizontalLane>();
    }

    public Vector2 GetCameraDestination(Vector2 newOffset)
    {
        Vector2 target;
        Vector2 playerPos = CameraScript.Player.transform.position;
        Vector2 curBehavObjPos = CameraScript.CameraBehaviourObject.transform.position;
        
        // Target X.
        if (CameraScript.GetStopAtLeft()) // stop at left = dont follow player if past behaviour's x
        {
            // only stop camera if player < behaviour position x
            if (playerPos.x > curBehavObjPos.x) 
            {
                target.x =  playerPos.x + newOffset.x;
            } 
            else 
            {
                target.x = cameraVertLane.CalcVertTarget(CameraScript.GetVerticalFollow());
            }
        }
        else
        {
            if (playerPos.x < curBehavObjPos.x) 
            {
                target.x =  playerPos.x + newOffset.x;
            }
            else 
            {
                target.x = cameraVertLane.CalcVertTarget(CameraScript.GetVerticalFollow());
            }
        }
        
        // Target Y.
        target.y = cameraHoriLane.CalcTargetY(newOffset);
        return target;
    }
}