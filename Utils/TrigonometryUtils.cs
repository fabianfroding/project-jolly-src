using UnityEngine;

public class TrigonometryUtils : MonoBehaviour
{
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
}
