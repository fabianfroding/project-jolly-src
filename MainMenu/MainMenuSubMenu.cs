using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuSubMenu : MonoBehaviour
{
    [SerializeField] protected GameObject parentMenuGO;
    [SerializeField] protected GameObject defaultSelectedButtonGO;
    [SerializeField] protected GameObject mainTitle;
    protected GameObject lastSelectedButtonGO;

    protected void Start()
    {
        SelectDefaultButton();
    }

    protected virtual void OnEnable()
    {
        if (parentMenuGO != null)
        {
            parentMenuGO.SetActive(false);
            mainTitle.SetActive(false);
        }
        else if (mainTitle != null)
        {
            mainTitle.SetActive(true);
        }

        SelectDefaultButton();
    }

    protected void SelectDefaultButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (lastSelectedButtonGO == null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedButtonGO);

            // For some reason the default start menu button doesn't get highlighted, this line fixed that.
            defaultSelectedButtonGO.GetComponent<Button>().OnSelect(null);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButtonGO);
            lastSelectedButtonGO.GetComponent<Button>().OnSelect(null);
        }
    }

    public virtual void DoneClicked()
    {
        if (parentMenuGO != null)
        {
            parentMenuGO.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    protected virtual void OpenNewMenuGameObject(GameObject menu)
    {
        lastSelectedButtonGO = EventSystem.current.currentSelectedGameObject;
        menu.SetActive(true);
    }
}
