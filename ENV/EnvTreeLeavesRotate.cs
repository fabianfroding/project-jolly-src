using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnvTreeLeavesRotate : MonoBehaviour
{
    [SerializeField] private float maxOffset = 15f;
    [SerializeField] private float increment = 0.8f;
    private float initialRotation;

    private void Awake()
    {
        //initialRotation = TransformUtils.GetInspectorRotation(transform).z;
        increment = Random.Range(0, 1) == 1 ? increment : -increment;
    }

    private void Update()
    {
#if UNITY_EDITOR
        // TODO: Change this to not use UnityEditor. It needs to work in builds too.
        if (increment > 0f && TransformUtils.GetInspectorRotation(transform).z > initialRotation + maxOffset ||
            increment < 0f && TransformUtils.GetInspectorRotation(transform).z < initialRotation - maxOffset)
        {
            increment = -increment;
        }
        transform.Rotate(Vector3.forward * (increment * Time.deltaTime));
#endif
    }
}
