using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayerHealthIcon : MonoBehaviour
{
    [SerializeField] private Image itemFill;

    public void FillItem() => itemFill.enabled = true;
    public void DepleteItem() => itemFill.enabled = false;
}
