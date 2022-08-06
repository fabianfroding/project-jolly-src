using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject selectionIndicatorGO;

    public static GameObject currentSelectedButton;

    public void OnEnter()
    {
        currentSelectedButton = gameObject;
        if (!selectionIndicatorGO.activeSelf)
        {
            selectionIndicatorGO.SetActive(true);
        }
    }

    public void OnExit()
    {
        if (selectionIndicatorGO.activeSelf)
        {
            selectionIndicatorGO.SetActive(false);
        }
    }

    public void OnSelectedMainMenuButtonClick()
    {
        if (currentSelectedButton != null)
        {
            Button button = currentSelectedButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
    }
}
