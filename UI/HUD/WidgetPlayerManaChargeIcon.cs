using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayerManaChargeIcon : MonoBehaviour
{
    [SerializeField] private Image itemFill;

    public void FillItem() => itemFill.enabled = true;
    public void DepleteItem() => itemFill.enabled = false;
}
