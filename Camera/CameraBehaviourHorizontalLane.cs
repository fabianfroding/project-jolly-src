using UnityEngine;

public class CameraBehaviourHorizontalLane : CameraBehaviour
{
    private Vector2 regionFST = new Vector2(0.9f, 0.4f);
    private float horiAdjPos;
    private Vector3 hitPos;

    public Vector2 GetRegionFST() => regionFST;

    public Vector2 GetCameraDestination(Vector2 newOffset)
    {
        Vector2 target;
        target.y = CalcTargetY(newOffset);
        target.x = CameraScript.Player.transform.position.x + newOffset.x;
        return target;
    }

    public float CalcTargetY(Vector2 newOffset)
    {
        float targetY;
        // lock axis depending on lane type, else follow like normal

        if (CameraScript.UsesTransformAsAnchor())
        {
            targetY = CameraScript.CameraBehaviourObject.transform.position.y;
        }
        else 
        {
            targetY = CalcHoriAdjTargetY(newOffset);
        }

        return targetY;
    }
    
    // Very annoying function that tries to keep player in frame in a dolly-like, adaptive way
    // shoots ray downwards and uses lowest ground as reference
    private float CalcHoriAdjTargetY(Vector2 newOffset)
    {
        float targetY;
        Vector3 dirToGround = Vector3.down.normalized;
        RaycastHit2D hit = Physics2D.Raycast(CameraScript.Player.transform.position, dirToGround, Mathf.Infinity, CameraScript.GetGroundMask());
        // in the air, just entered collider
        if (horiAdjPos == 0)
        {
            CalcInitialHoriAdjPosition(dirToGround, hit);
            targetY = horiAdjPos;

            if (!hit) { targetY = CameraScript.Player.transform.position.y + newOffset.y; }
        }
        else {
            UpdateHoriAdjPosition(dirToGround, hit);
            targetY = horiAdjPos;
        }
        return targetY;
    }

    private void CalcInitialHoriAdjPosition(Vector3 dirToGround, RaycastHit2D hit)
    {
        if (!hit) {
            Debug.Log("no hit?");
        }
        else
        {
            // fire ray down, check position of ground, calculate adjustment 
            GameObject hitObject = hit.collider.gameObject;
            hitPos = hit.point;

            if (CameraScript.GetHorizontalPanCameraUp()) 
            {   horiAdjPos = hit.point.y + CameraScript.GetHorizontalYAdjustment();  
                // print("panning up");
            }
            else 
            {   horiAdjPos = hit.point.y - CameraScript.GetHorizontalYAdjustment();  
                // print("panning down");
            }
        }
    }
    // man i have no idea wtf is goin on here
    private void UpdateHoriAdjPosition(Vector3 dirToGround, RaycastHit2D hit)
    {
        float adjustmentLeeway = 3;
        // if lower ground exists go there
        if (hit && CameraScript.GetHorizontalPanCameraUp()) 
        {   
            if (hit.point.y + CameraScript.GetHorizontalYAdjustment() < horiAdjPos - adjustmentLeeway) {
                horiAdjPos = hit.point.y + CameraScript.GetHorizontalYAdjustment();
                hitPos = hit.point;
            } 
        }
        else {
            if (hit.point.y - CameraScript.GetHorizontalYAdjustment() > horiAdjPos + adjustmentLeeway) {
                horiAdjPos = hit.point.y - CameraScript.GetHorizontalYAdjustment();
                hitPos = hit.point;
            } 
        }
    }

    public void ResetHoriAdjPos()
    {
        horiAdjPos = 0;
    }
}