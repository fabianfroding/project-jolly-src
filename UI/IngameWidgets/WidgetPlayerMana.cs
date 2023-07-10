using UnityEngine;
using UnityEngine.UI;

public class WidgetPlayerMana : MonoBehaviour
{
    public Image progressBar;

    private void Awake()
    {
        StatsPlayer.OnPlayerManaChange += SetMana;
    }

    public void SetMana(float progress)
    {
        progressBar.fillAmount = progress * 0.01f;
    }
}
