using System;
using System.Collections;
using UnityEngine;

public class NPC : Entity
{
    public static event Action OnNPCRegisterToNPCNotebook;

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();

        FungusEventHandler.OnExitFungusDialogue += ExitDialogue;

        // Prevent NPCs from colliding with player or enemies.
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_PLAYER));
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(EditorConstants.LAYER_NPC), LayerMask.NameToLayer(EditorConstants.LAYER_ENEMY));
    }

    protected virtual void OnDestroy()
    {
        FungusEventHandler.OnExitFungusDialogue -= ExitDialogue;
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
}
