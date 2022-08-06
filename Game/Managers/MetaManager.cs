using UnityEngine;

public class MetaManager : MonoBehaviour
{
    [SerializeField] private GameObject uiManagerPrefab;

    private void Awake()
    {
        AddCurrencyManager();
        AddEquipmentManager();
        AddFungusEventHandler();
        AddInGameProfileManager();
        AddSaveManager();
        AddUIManager();
    }

    private void AddCurrencyManager()
    {
        GameObject tempGO = AddChild("CurrencyManager");
        tempGO.AddComponent<CurrencyManager>();
    }

    private void AddEquipmentManager()
    {
        GameObject tempGO = AddChild("EquipmentManager");
        tempGO.AddComponent<EquipmentManager>();
    }

    private void AddFungusEventHandler()
    {
        GameObject tempGO = AddChild("FungusEventHandler");
        tempGO.AddComponent<FungusEventHandler>();
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

    private void AddUIManager()
    {
        GameObject tempGO = Instantiate(uiManagerPrefab);
        tempGO.transform.parent = transform;
    }

    private GameObject AddChild(string name)
    {
        GameObject tempGO = new GameObject(name);
        tempGO.transform.parent = transform;
        return tempGO;
    }
}
