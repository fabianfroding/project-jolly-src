using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject widgetHUD;

    private void Awake()
    {
        AddCurrencyManager();
        AddDaytimeManager();
        AddEquipmentManager();
        AddInGameProfileManager();
        AddSaveManager();
        AddWidgetHUD();
    }

    private void AddCurrencyManager()
    {
        GameObject tempGO = AddChild("CurrencyManager");
        tempGO.AddComponent<CurrencyManager>();
    }

    private void AddDaytimeManager()
    {
        GameObject tempGO = AddChild("DaytimeManager");
        tempGO.AddComponent<DaytimeManager>();
    }

    private void AddEquipmentManager()
    {
        GameObject tempGO = AddChild("EquipmentManager");
        tempGO.AddComponent<EquipmentManager>();
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
        GameObject tempGO = new GameObject(name);
        tempGO.transform.parent = transform;
        return tempGO;
    }
}
