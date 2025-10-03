using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayerMana : MonoBehaviour
{
    [SerializeField] private Slider widgetPlayerManaFill;

    [SerializeField] private int playerMana;

    public void OnPlayerManaChanged()
    {
        widgetPlayerManaFill.value = playerMana;
    }
}
