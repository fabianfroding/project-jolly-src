using TMPro;
using UnityEngine;

public class WidgetInteractionPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private SOInteractionData interactionData;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SetText();
    }

    public void SetText()
    {
        if (!Text) return;
        if (!interactionData) return;
        if (interactionData.interactable == null) return;
        Text.text = interactionData.interactable.GetInteractionText();
    }
}
