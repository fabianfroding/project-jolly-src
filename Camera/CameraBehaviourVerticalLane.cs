using UnityEngine;

public class CameraBehaviourVerticalLane : CameraBehaviour
{
    // Player-behaviour lerp t-value = target is in between player and behaviour object
    private float pbLerpTValue = 0.2f;

    public Vector2 GetCameraDestination(Vector2 newOffset)
    {
        Vector2 target;
        
        target.x = CalcVertTarget(CameraScript.GetVerticalFollow());
        target.y = CameraScript.Player.transform.position.y + newOffset.y;

        return target;
    }

    public float CalcVertTarget(bool smolVertFollow) {
        Vector2 vertTarget;
        float targetX;

        if (smolVertFollow) {
            // slightly follow player on x
            vertTarget = CalcPlayerPOITarget();
            targetX =  vertTarget.x;
        }
        else {
            targetX =  CameraScript.CameraBehaviourObject.transform.position.x;
        } 
        return targetX;
    }

    public Vector2 CalcPlayerPOITarget() 
    {
        // first find target vector - a point in between the player and the POI.
        // behaviourTValue makes it really close to the POI, but since the player pos changes it still slightly follows the player 
        Vector2 target = Vector2.zero;

        target = Vector2.Lerp(
            new Vector3(
                CameraScript.CameraBehaviourObject.transform.position.x,
                CameraScript.CameraBehaviourObject.transform.position.y
            ),
            new Vector2(
                CameraScript.Player.transform.position.x,
                CameraScript.Player.transform.position.y
            ),
            pbLerpTValue
        );

        return target;
    }
}