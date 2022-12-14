using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(1, 20)]
    [SerializeField] private float viewRadius;
    [Range(1, 360)]
    [SerializeField] private float viewWidth;

    [Tooltip("Horizontal view direction automatically adjust to facing direction.")]
    [SerializeField] private VIEW_DIRECTION viewDirection;

    [SerializeField] private Transform fovOrigin;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstructionLayer;

    public enum VIEW_DIRECTION { HORIZONTAL, UP, DOWN }
    private Vector2 viewDirectionVector;
    private readonly float tickInterval = 0.2f;

    public bool CanSeeTarget { get; private set; }
    public GameObject Target { get; private set; }

    #region Unity Callback Functions
    private void Start()
    {
        if (fovOrigin == null)
        {
            fovOrigin = transform;
        }

        switch (viewDirection)
        {
            case VIEW_DIRECTION.HORIZONTAL:
                viewDirectionVector = new Vector2(-1, 0);
                break;
            case VIEW_DIRECTION.UP:
                viewDirectionVector = new Vector2(0, -1);
                break;
            case VIEW_DIRECTION.DOWN:
                viewDirectionVector = new Vector2(0, 1);
                break;
            default:
                viewDirectionVector = new Vector2(-1, 0);
                break;
        }

        StartCoroutine(FOVCheck());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(fovOrigin.position, Vector3.forward, viewRadius);

        // Instead of creating a coupling to Core (which won't even work in editor mode),
        // we can rely on transform.rotiation.y since that is the field that the core movement modifies.
        float transformDir;
        if (viewDirection == VIEW_DIRECTION.UP)
        {
            transformDir = 90f;
        }
        else if (viewDirection == VIEW_DIRECTION.DOWN)
        {
            transformDir = -90f;
        }
        else
        {
            transformDir = transform.rotation.y >= 0 ? 0f : 180f;
        }
        Vector3 angle1 = TrigonometryUtils.GetDirectionFromAngle(-viewWidth / 2 + transformDir);
        Vector3 angle2 = TrigonometryUtils.GetDirectionFromAngle(viewWidth / 2 + transformDir);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(fovOrigin.position, fovOrigin.position + angle1 * viewRadius);
        Gizmos.DrawLine(fovOrigin.position, fovOrigin.position + angle2 * viewRadius);

        if (CanSeeTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(fovOrigin.position, Target.transform.position);
        }
    }
    #endregion

    #region Other Functions
    // Using coroutine instead of Update to save performance.
    private IEnumerator FOVCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickInterval);
            FOV();
        }
    }

    private void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(fovOrigin.position, viewRadius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Target = rangeCheck[0].gameObject;
            Vector2 dirToTarget = TrigonometryUtils.GetDirectionBetweenPositions(transform, Target.transform);

            float angle = viewDirectionVector.y != 0 ?
                Vector2.Angle(-viewDirectionVector, dirToTarget) :
                Vector2.Angle(transform.rotation.y >= 0 ? -viewDirectionVector : viewDirectionVector, dirToTarget);

            if (angle < viewWidth / 2)
            {
                float distToTarget = Vector2.Distance(transform.position, Target.transform.position);
                if (!Physics2D.Raycast(fovOrigin.position, dirToTarget, distToTarget, obstructionLayer))
                {
                    CanSeeTarget = true;
                }
                else
                {
                    ResetTarget();
                }
            }
            else
            {
                ResetTarget();
            }
        }
        else if (CanSeeTarget)
        {
            ResetTarget();
        }
    }

    private void ResetTarget()
    {
        CanSeeTarget = false;
        Target = null;
    }
    #endregion
}
