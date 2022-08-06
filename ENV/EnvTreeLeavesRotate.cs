using UnityEditor;
using UnityEngine;

public class EnvTreeLeavesRotate : MonoBehaviour
{
    [SerializeField] private float maxOffset = 15f;
    [SerializeField] private float increment = 0.8f;
    private float initialRotation;

    private void Awake()
    {
        initialRotation = TransformUtils.GetInspectorRotation(transform).z;
        increment = Random.Range(0, 1) == 1 ? increment : -increment;
    }

    private void Update()
    {
        if (increment > 0f && TransformUtils.GetInspectorRotation(transform).z > initialRotation + maxOffset ||
            increment < 0f && TransformUtils.GetInspectorRotation(transform).z < initialRotation - maxOffset)
        {
            increment = -increment;
        }
        transform.Rotate(Vector3.forward * (increment * Time.deltaTime));
    }
}
