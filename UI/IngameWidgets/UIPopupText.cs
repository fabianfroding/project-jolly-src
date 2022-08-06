using System.Collections;
using UnityEngine;

public class UIPopupText : UIIngameWidget
{
    [Range(1, 10)]
    [Tooltip("Determines how long the UI should be displayed. This duration is refreshed if triggered multiple times.")]
    [SerializeField] protected float displayDuration = 4f;

    protected virtual void Start()
    {
        StartDestroyAfterDelay();
    }

    protected void OnEnable()
    {
        StartDestroyAfterDelay();
    }

    protected void StartDestroyAfterDelay()
    {
        StopCoroutine(DestroyAfterDelay());
        StartCoroutine(DestroyAfterDelay());
    }

    protected virtual IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        Destroy(gameObject);
    }
}
