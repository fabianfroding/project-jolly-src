using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINPCNotebookEntry : MonoBehaviour
{
    [SerializeField] private Image listImage;
    private NPCNotebookEntry entry;

    public void SetEntry(NPCNotebookEntry entry)
    {
        this.entry = entry;
        listImage.sprite = entry.sprite;
    }
}
