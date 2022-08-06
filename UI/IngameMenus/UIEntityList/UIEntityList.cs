using System.Collections;
using UnityEngine;

public class UIEntityList : MonoBehaviour
{
    protected readonly int previewIndex = 2;
    protected int currentIndex = 0;
    protected bool isMoving = false;

    protected virtual void Start()
    {
        UpdateListView();
    }

    public virtual void UpdateListView() {}

    public virtual void MoveSelectionUp()
    {
        if (!isMoving)
        {
            isMoving = true;
            if (currentIndex > 0) currentIndex--;
            UpdateListView();
            StopCoroutine(ResetIsMoving());
            StartCoroutine(ResetIsMoving());
        }
    }

    public virtual void MoveSelectionDown()
    {
        if (!isMoving)
        {
            isMoving = true;
            UpdateListView();
            StopCoroutine(ResetIsMoving());
            StartCoroutine(ResetIsMoving());
        }
    }

    protected virtual IEnumerator ResetIsMoving()
    {
        yield return new WaitForSeconds(0.2f);
        isMoving = false;
    }
}
