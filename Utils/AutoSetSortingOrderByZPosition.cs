using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class AutoSetSortingOrderByZPosition : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SortingGroup sortGroup;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortGroup = GetComponent<SortingGroup>();

#if UNITY_STANDALONE && !UNITY_EDITOR
        UpdateSortingOrder();
#endif
    }

#if UNITY_EDITOR
    private void Update()
    {
        UpdateSortingOrder();
    }
#endif

    private void UpdateSortingOrder()
    {
        if (sortGroup)
            sortGroup.sortingOrder = (int)(-10 * transform.position.z);
        else if (spriteRenderer)
            spriteRenderer.sortingOrder = (int)(-10 * transform.position.z);
    }
}
