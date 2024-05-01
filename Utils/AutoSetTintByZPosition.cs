using UnityEngine;

[ExecuteInEditMode]
public class AutoSetTintByZPosition : MonoBehaviour
{
    [Tooltip("At which Z-position the sprite should become pitch black. " +
        "Z-positions between 0 and this value will adapt the tint between default color and black. " +
        "Z-positions less than 0 will always bet the default color.")]
    [SerializeField] private SOFloatVariable autoTintThreshold;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

#if UNITY_STANDALONE && !UNITY_EDITOR
        UpdateTint();
#endif
    }

#if UNITY_EDITOR
    private void Update() => UpdateTint();
#endif

    private void UpdateTint()
    {
        if (spriteRenderer == null || !autoTintThreshold)
            return;

        float zPosition = transform.position.z;
        if (zPosition >= 0)
            spriteRenderer.color = Color.white;
        else if (zPosition > autoTintThreshold.Value)
            spriteRenderer.color = Color.Lerp(Color.white, Color.black, zPosition / autoTintThreshold.Value);
        else
            spriteRenderer.color = Color.black;
    }
}
