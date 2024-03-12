using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject widgetHUD;

    private void Awake()
    {
        AddInGameProfileManager();
        AddSaveManager();
        AddWidgetHUD();
    }

    private void AddInGameProfileManager()
    {
        GameObject tempGO = AddChild("InGameProfileManager");
        tempGO.AddComponent<InGameProfileManager>();
    }

    private void AddSaveManager()
    {
        GameObject tempGO = AddChild("SaveManager");
        tempGO.AddComponent<SaveManager>();
    }

    private void AddWidgetHUD()
    {
        GameObject tempGO = Instantiate(widgetHUD);
        tempGO.transform.SetParent(transform);
    }

    private GameObject AddChild(string name)
    {
        GameObject tempGO = new(name);
        tempGO.transform.parent = transform;
        return tempGO;
    }
}
