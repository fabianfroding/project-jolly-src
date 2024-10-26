using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayerMana : MonoBehaviour
{
    [SerializeField] private Slider widgetPlayerManaFill;

    [SerializeField] private SOIntVariable playerMana;

    public void OnPlayerManaChanged()
    {
        widgetPlayerManaFill.value = playerMana.Value;
    }
}
