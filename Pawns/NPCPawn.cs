using UnityEngine;

public class NPCPawn : PawnBase, IInteractable
{
    [SerializeField] private Types.DialogueData[] dialogues;
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private GameObject interactionIndicatorGO;

    private int currentDialogueIndex;
    private int currentDialogueParagraphIndex;

    protected override void Awake()
    {
        base.Awake();
        currentDialogueIndex = 0; // TODO: Get this from save data.
        currentDialogueParagraphIndex = 0;

        // Prevent NPCs from colliding with player or enemies.
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY));
    }

    #region Interactable Interface
    public bool AdvanceInteraction() =>
        currentDialogueParagraphIndex++ < dialogues[currentDialogueIndex].paragraphs.Length - 1;

    public void Interact() => currentDialogueParagraphIndex = 0;
    public void InteractEnd() => currentDialogueIndex = Mathf.Clamp(currentDialogueIndex + 1, 0, dialogues.Length - 1);
    public bool IsInteractable() => isInteractable;

    public string GetInteractionText()
    {
        string result = "Missing interaction text.";

        for (int i = 0; i < dialogues.Length; i++)
        {
            if (i == currentDialogueIndex)
            {
                for (int j = 0; j < dialogues[i].paragraphs.Length; j++)
                {
                    if (j == currentDialogueParagraphIndex)
                        result = dialogues[i].paragraphs[j];
                }
            }
        }

        return result;
    }
    #endregion
}
