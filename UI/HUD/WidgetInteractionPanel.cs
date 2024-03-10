using TMPro;
using UnityEngine;

public class WidgetInteractionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        if (Text)
            Text.text = text;
    }
}
