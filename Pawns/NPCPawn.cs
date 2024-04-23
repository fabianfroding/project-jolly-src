using UnityEngine;

public class NPCPawn : PawnBase, IInteractable
{
    [SerializeField] D_InteractData[] interactData;
    private int interactDataIndex = 0;
    private int interactDataTextIndex = 0;

    protected override void Awake()
    {
        base.Awake();

        // Prevent NPCs from colliding with player or enemies.
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY));
    }

    public int GetInteractDataIndex() => interactDataIndex;

    #region Interactable Interface
    public bool AdvanceInteraction()
    {
        for (int i = 0; i < interactData[interactDataIndex].InteractText.Length; i++)
        {
            if (i == interactDataTextIndex)
            {
                WidgetHUD widgetHUD = WidgetHUD.Instance;
                if (widgetHUD != null)
                {
                    WidgetHUD.Instance.SetInteractionPanelText(interactData[interactDataIndex].InteractText[interactDataTextIndex]);
                }
                interactDataTextIndex++;
                return true;
            }
        }
        interactDataIndex = Mathf.Clamp(interactDataIndex + 1, 0, interactData.Length - 1);
        return false;
    }
    public void Interact()
    {
        WidgetHUD widgetHUD = WidgetHUD.Instance;
        if (widgetHUD != null)
        {
            interactDataTextIndex = 0;
            WidgetHUD.Instance.ShowInteractionPanel(true);
            AdvanceInteraction();
        }
    }

    public bool IsInteractable() => false;
    #endregion
}
