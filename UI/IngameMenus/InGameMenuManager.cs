using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] inGameMenus;
    private GameObject currentMenu;
    private int currentMenuIndex = 0;
    private static int lastActiveMenuIndex = 0;

    private static InGameMenuManager instance;
    public static InGameMenuManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<InGameMenuManager>();
            return instance;
        }
    }

    #region Unity Callback Functions
    private void Start()
    {
        currentMenuIndex = lastActiveMenuIndex;
        InstantiateMenu(inGameMenus[currentMenuIndex]);
    }

    private void OnDisable()
    {
        lastActiveMenuIndex = currentMenuIndex;
        Destroy(currentMenu);
    }
    #endregion

    private void InstantiateMenu(GameObject menu)
    {
        currentMenu = Instantiate(inGameMenus[currentMenuIndex]);
    }

    private void AdjustCurrentMenuIndex()
    {
        if (currentMenuIndex >= inGameMenus.Length) currentMenuIndex = 0;
        else if (currentMenuIndex < 0) currentMenuIndex = inGameMenus.Length - 1;
    }

    public void InGameMenuSelect()
    {
        EquipmentManager equipmentManager = FindObjectOfType<EquipmentManager>();
        if (equipmentManager != null)
        {
            equipmentManager.EquipItem();
            return;
        }
    }

    public void ShiftRight()
    {
        Destroy(currentMenu);
        currentMenuIndex++;
        AdjustCurrentMenuIndex();
        InstantiateMenu(inGameMenus[currentMenuIndex]);
    }

    public void ShiftLeft()
    {
        Destroy(currentMenu);
        currentMenuIndex--;
        AdjustCurrentMenuIndex();
        InstantiateMenu(inGameMenus[currentMenuIndex]);
    }

    public void MoveInGameMenuSelectionUp()
    {
        UIEntityList inGameMenuEntityList = FindObjectOfType<UIEntityList>();
        if (inGameMenuEntityList != null)
        {
            inGameMenuEntityList.MoveSelectionUp();
            return;
        }

        UIEquipmentMenu uiEquipmentMenu = FindObjectOfType<UIEquipmentMenu>();
        if (uiEquipmentMenu != null)
        {
            uiEquipmentMenu.MoveSelectionUp();
            return;
        }
    }

    public void MoveInGameMenuSelectionDown()
    {
        UIEntityList inGameMenuEntityList = FindObjectOfType<UIEntityList>();
        if (inGameMenuEntityList != null)
        {
            inGameMenuEntityList.MoveSelectionDown();
            return;
        }

        UIEquipmentMenu uiEquipmentMenu = FindObjectOfType<UIEquipmentMenu>();
        if (uiEquipmentMenu != null)
        {
            uiEquipmentMenu.MoveSelectionDown();
            return;
        }
    }

    public void MoveInGameMenuSelectionLeft()
    {
        UIEquipmentMenu uiEquipmentMenu = FindObjectOfType<UIEquipmentMenu>();
        if (uiEquipmentMenu != null)
        {
            uiEquipmentMenu.MoveSelectionLeft();
            return;
        }
    }

    public void MoveInGameMenuSelectionRight()
    {
        UIEquipmentMenu uiEquipmentMenu = FindObjectOfType<UIEquipmentMenu>();
        if (uiEquipmentMenu != null)
        {
            uiEquipmentMenu.MoveSelectionRight();
            return;
        }
    }
}
