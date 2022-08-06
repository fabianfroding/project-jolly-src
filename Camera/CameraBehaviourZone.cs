using System;
using UnityEngine;

public class CameraBehaviourZone : MonoBehaviour
{
    [SerializeField] private CameraBehaviour cameraBehaviour = CameraBehaviour.HorizontalLane;

    [Header("Horizontal Attributes")]
    [SerializeField] private bool useTransformAsAnchor = false;
    [Tooltip("False pans camera down - only for adjustment.")]
    [SerializeField] private bool horiPanCameraUp = true;
    [SerializeField] private float horizontalYAdjustment = 5.5f;

    [Header("Vertical Attributes")]
    [Tooltip("The slight horizontal camera pan.")]
    [SerializeField] private bool vertFollow = true;
    [Tooltip("If false stops at right.")]
    [SerializeField] private bool horiVertStopAtLeft = true;

    public static event Action<GameObject> OnEnterCameraBehaviourZone;
    public static event Action<GameObject> OnExitCameraBehaviourZone;

    public enum CameraBehaviour
    {
        HorizontalLane,
        VerticalLane,
        CrossLane,
        PointOfInterest
    }

    #region Unity Callback Functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            OnEnterCameraBehaviourZone?.Invoke(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(EditorConstants.TAG_PLAYER))
        {
            OnExitCameraBehaviourZone?.Invoke(null);
        }
    }
    #endregion

    public CameraBehaviour GetCameraBehaviour() => cameraBehaviour;
    public bool UsesTransformAsAnchor() => useTransformAsAnchor;
    public bool GetHorizontalPanCameraUp() => horiPanCameraUp;
    public bool GetVerticalFollow() => vertFollow;
    public bool GetStopAtLeft() => horiVertStopAtLeft;
    public float GetHorizontalYAdjustment() => horizontalYAdjustment;
}
