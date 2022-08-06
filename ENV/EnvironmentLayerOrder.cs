using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class EnvironmentLayerOrder : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    SortingGroup sortGroup;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortGroup = GetComponent<SortingGroup>();
    }

    private void Update()
    {
        if (sortGroup)
        {
            sortGroup.sortingOrder = (int)(-10 * transform.position.z);
        }
        else
        {
            spriteRenderer.sortingOrder = (int)(-10 * transform.position.z);
        }
    }

    /*private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
            SetOrderInLayer();
    }

    private void SetOrderInLayer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sortingOrder != -(int)(transform.position.z * 100f))
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.z * 100f);
            //Debug.Log(gameObject.name + "'s order in layer set to " + spriteRenderer.sortingOrder);
        }
    }*/
}
