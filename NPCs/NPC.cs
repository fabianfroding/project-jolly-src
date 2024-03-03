using System;
using System.Collections;
using UnityEngine;

public class NPC : Entity, IInteractable
{
    [SerializeField] D_InteractData[] interactData;
    private int interactDataIndex = 0;
    private int interactDataTextIndex = 0;

    public static event Action OnNPCRegisterToNPCNotebook;

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();

        // Prevent NPCs from colliding with player or enemies.
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY));
    }
    #endregion

    #region Other Functions
    protected void ExitDialogue()
    {
        if (!NPCNotebookRepository.IsNPCRegisteredInNPCNotebook(name))
        {
            NPCNotebookRepository.SaveNPCToNPCNotebook(name);
            StopCoroutine(InvokeOnNPCRegisterToNPCNotebook());
            StartCoroutine(InvokeOnNPCRegisterToNPCNotebook());
        }
    }

    protected IEnumerator InvokeOnNPCRegisterToNPCNotebook()
    {
        yield return new WaitForSeconds(1.5f);
        OnNPCRegisterToNPCNotebook?.Invoke();
    }
    #endregion

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
        interactDataIndex = Mathf.Clamp(interactDataIndex + 1, 0, interactData.Length - 1); // TODO: Needs to be saved to remember the dialogue index next time game is restarted.
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
