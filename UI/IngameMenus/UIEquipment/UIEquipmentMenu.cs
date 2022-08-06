using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIEquipmentMenu : MonoBehaviour
{
    private const int COLS = 6;
    private const int ROWS = 4;
    [SerializeField] private GameObject[,] grid;
    [SerializeField] private GameObject[] rowEquipped;
    [SerializeField] private GameObject[] rowCollection1;
    [SerializeField] private GameObject[] rowCollection2;
    [SerializeField] private GameObject[] rowCollection3;
    [SerializeField] private GameObject selector;

    [SerializeField] private TextMeshProUGUI previewText;
    [SerializeField] private Image previewImage;
    [SerializeField] private TextMeshProUGUI previewDescription;

    private Vector2 posIndex;
    private GameObject currentSlot;
    private bool isMoving = false;
    private int equippedIndex = 0;

    #region Unity Callback Functions
    private void Awake()
    {
        grid = new GameObject[ROWS, COLS];
    }

    private void Start()
    {
        AddRowToGrid(0, rowEquipped);
        AddRowToGrid(1, rowCollection1);
        AddRowToGrid(2, rowCollection2);
        AddRowToGrid(3, rowCollection3);

        posIndex = new Vector2(0, 0);
        currentSlot = grid[0, 0];
    }

    private void OnEnable()
    {
        EquipmentManager.OnEquipItem += EquipSelected;
    }

    private void OnDisable()
    {
        EquipmentManager.OnEquipItem -= EquipSelected;
    }
    #endregion

    private void AddRowToGrid(int index, GameObject[] row)
    {
        for (int i = 0; i < row.Length; i++) grid[index, i] = row[i];
    }

    #region Selection Movement
    public void MoveSelection()
    {
        if (posIndex.y >= ROWS) posIndex.y = 0;
        else if (posIndex.y < 0) posIndex.y = ROWS - 1;
        else if (posIndex.x >= COLS) posIndex.x = 0;
        else if (posIndex.x < 0) posIndex.x = COLS - 1;

        currentSlot = grid[(int)posIndex.y, (int)posIndex.x];
        selector.transform.position = currentSlot.transform.position;

        SetPreview();
        StopCoroutine(ResetIsMoving());
        StartCoroutine(ResetIsMoving());
    }

    private IEnumerator ResetIsMoving()
    {
        yield return new WaitForSeconds(0.2f);
        isMoving = false;
    }

    public void MoveSelectionUp()
    {
        if (!isMoving)
        {
            isMoving = true;
            posIndex.y--;
            MoveSelection();
        }
    }

    public void MoveSelectionDown()
    {
        if (!isMoving)
        {
            isMoving = true;
            posIndex.y++;
            MoveSelection();
        }
    }

    public void MoveSelectionLeft()
    {
        if (!isMoving)
        {
            isMoving = true;
            posIndex.x--;
            MoveSelection();
        }
    }

    public void MoveSelectionRight()
    {
        if (!isMoving)
        {
            isMoving = true;
            posIndex.x++;
            MoveSelection();
        }
    }
    #endregion

    public void SetPreview()
    {
        if (currentSlot.GetComponent<UIEquipmentSlot>().EquipmentItem != null && currentSlot.GetComponent<UIEquipmentSlot>().EquipmentItem.obtained)
        {
            previewText.text = currentSlot.GetComponent<UIEquipmentSlot>().EquipmentItem.itemName;
            previewImage.gameObject.SetActive(true);
            previewImage.GetComponent<Image>().sprite = currentSlot.GetComponent<UIEquipmentSlot>().EquipmentItem.itemSprite;
            previewDescription.text = currentSlot.GetComponent<UIEquipmentSlot>().EquipmentItem.itemDescription;
        }
        else
        {
            previewText.text = "";
            previewImage.gameObject.SetActive(false);
            previewDescription.text = "";
        }
    }

    public void EquipSelected()
    {
        UIEquipmentSlot currentEquipmentSlot = currentSlot.GetComponent<UIEquipmentSlot>();
        EquipmentItem currentEquipmentItem = currentEquipmentSlot.EquipmentItem;

        if (currentEquipmentSlot != null && currentEquipmentItem != null)
        {
            if (posIndex.y == 0 && equippedIndex >= 0)
            {
                // Unequip.
                UIEquipmentSlot equipmentSlot = currentSlot.GetComponent<UIEquipmentSlot>();
                UIEquipmentSlot originalSlot = equipmentSlot.EquipmentItem.originalSlot.GetComponent<UIEquipmentSlot>();

                originalSlot.EquipmentItem = equipmentSlot.EquipmentItem;
                originalSlot.GetComponent<Image>().sprite = equipmentSlot.EquipmentItem.itemSprite;
                originalSlot.GetComponent<Image>().color = Color.white;

                equipmentSlot.EquipmentItem = null;
                equipmentSlot.equipmentItemPrefab = null;
                equipmentSlot.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                equipmentSlot.GetComponent<Image>().sprite = null;

                equippedIndex--;
            }
            else if (posIndex.y > 0 && equippedIndex < COLS)
            {
                // Equip.
                UIEquipmentSlot equipmentSlot = rowEquipped[equippedIndex].GetComponent<UIEquipmentSlot>();
                equipmentSlot.equipmentItemPrefab = currentEquipmentSlot.equipmentItemPrefab;
                equipmentSlot.EquipmentItem = currentEquipmentItem;
                equipmentSlot.GetComponent<Image>().sprite = currentEquipmentItem.itemSprite;
                equipmentSlot.GetComponent<Image>().color = Color.white;
                equipmentSlot.EquipmentItem.originalSlot = currentSlot.gameObject;

                currentEquipmentSlot.EquipmentItem = null;
                currentEquipmentSlot.equipmentItemPrefab = null;
                currentEquipmentSlot.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                currentEquipmentSlot.GetComponent<Image>().sprite = null;

                equippedIndex++;
            }
        }
        SetPreview();
    }
}
