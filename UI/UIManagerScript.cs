using TMPro;
using UnityEngine;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject uiCurrencyPrefab;
    [SerializeField] private GameObject uiInGameMenusPrefab;
    [SerializeField] private GameObject uiNPCNotebookNewEntryPrefab;
    [SerializeField] private GameObject uiPlayerHealthPrefab;
    [SerializeField] private GameObject uiPlayerManaPrefab;
    [SerializeField] private GameObject uiPowerupObtainedPrefab;
    [SerializeField] private GameObject uiSaveGamePrefab;
    [SerializeField] private GameObject widgetDebugMenuPrefab;

    private GameObject inGameMenusUI;
    private bool isInGameMenusUIActive = false;
    UIIngameWidget[] inGameUIWidgets;

    private GameObject widgetDebugMenu;
    private bool widgetDebugMenuActive = false;

    private static UIManagerScript instance;
    public static UIManagerScript Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIManagerScript>();
            return instance;
        }
    }

    #region Unity Callback Functions
    private void Awake()
    {
        CurrencyManager.OnCurrencyChange += InstantiateCurrencyUI;
        NPC.OnNPCRegisterToNPCNotebook += InstantiateNPCNotebookNewEntryUI;
        Pickup_Powerup.OnPickupPowerup += InstantiatePowerupObtainedUI;
        SaveManager.OnGameSave += InstantiateSaveGameUI;

        GameObject uiPlayerHealth = Instantiate(uiPlayerHealthPrefab);
        uiPlayerHealth.transform.SetParent(transform);
        GameObject uiPlayerMana = Instantiate(uiPlayerManaPrefab);
        uiPlayerMana.transform.SetParent(transform);
    }

    private void OnDestroy()
    {
        CurrencyManager.OnCurrencyChange -= InstantiateCurrencyUI;
        NPC.OnNPCRegisterToNPCNotebook -= InstantiateNPCNotebookNewEntryUI;
        SaveManager.OnGameSave -= InstantiateSaveGameUI;
    }
    #endregion

    #region Other Functions
    private void InstantiateCurrencyUI() => InstantiateUI(uiCurrencyPrefab);
    private void InstantiateNPCNotebookNewEntryUI() => InstantiateUI(uiNPCNotebookNewEntryPrefab);
    private void InstantiatePowerupObtainedUI(string text)
    {
        GameObject tempGO = InstantiateUI(uiPowerupObtainedPrefab);
        if (tempGO != null)
        {
            TextMeshProUGUI tmp = tempGO.GetComponent<TextMeshProUGUI>();
            if (tmp != null) tmp.text = text;
        }
    }
    private void InstantiateSaveGameUI() => InstantiateUI(uiSaveGamePrefab);

    private GameObject InstantiateUI(GameObject ui) => ui != null ? Instantiate(ui) : null;

    public void ToggleInGameMenusUI()
    {
        if (inGameMenusUI == null && !isInGameMenusUIActive)
        {
            isInGameMenusUIActive = true;
            inGameMenusUI = Instantiate(uiInGameMenusPrefab);
            ShowInGameUIWidgets(false);
        }
        else if (isInGameMenusUIActive)
        {
            inGameMenusUI.SetActive(false);
            Destroy(inGameMenusUI);
            inGameMenusUI = null;
            isInGameMenusUIActive = false;
            ShowInGameUIWidgets(true);
        }
    }

    private void ShowInGameUIWidgets(bool show)
    {
        UIIngameWidget[] inGameUIWidgets;
        if (this.inGameUIWidgets == null || !show)
        {
            this.inGameUIWidgets = FindObjectsOfType<UIIngameWidget>();
            inGameUIWidgets = this.inGameUIWidgets;
        }
        else
        {
            inGameUIWidgets = this.inGameUIWidgets;
        }
        for (int i = 0; i < inGameUIWidgets.Length; i++)
        {
            inGameUIWidgets[i].gameObject.SetActive(show);
        }
    }

    public void ToggleDebugMenu()
    {
        if (widgetDebugMenu == null && !widgetDebugMenuActive)
        {
            widgetDebugMenuActive = true;
            widgetDebugMenu = Instantiate(widgetDebugMenuPrefab);
            ShowInGameUIWidgets(false);
        }
        else if (widgetDebugMenuActive)
        {
            widgetDebugMenu.SetActive(false);
            Destroy(widgetDebugMenu);
            widgetDebugMenu = null;
            widgetDebugMenuActive = false;
            ShowInGameUIWidgets(true);
        }
    }
    #endregion
}
