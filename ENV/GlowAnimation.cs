using UnityEngine;

public class GlowAnimation : MonoBehaviour
{
    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private float maxSize = 0.37f;
    [SerializeField] [Range(0.01f, 0.99f)] private float growthSpeed = 0.01f;
    private float growModifier = 0;

    //==================== PRIVATE ====================//
    private void Update()
    {
        if (transform.localScale.x >= maxSize && growModifier >= 0) growModifier = -(growthSpeed / 50f);
        else if (transform.localScale.x <= minSize && growModifier < 0) growModifier = growthSpeed / 50f;
        transform.localScale = new Vector2(transform.localScale.x + growModifier, transform.localScale.y + growModifier);
    }
}
