using System;
using UnityEngine;

public class FungusEventHandler : MonoBehaviour
{
    public static event Action OnExitFungusDialogue;

    private void Awake()
    {
        FungusCommandEnterDialogue.OnEnterFungusDialogue += EnterFungusDialogue;
        FungusCommandExitDialogue.OnExitFungusDialogue += ExitFungusDialogue;
        PlayerInputHandler.OnTriggerFungusDialog += TriggerFungusDialog;
    }

    private void OnDisable()
    {
        FungusCommandEnterDialogue.OnEnterFungusDialogue -= EnterFungusDialogue;
        FungusCommandExitDialogue.OnExitFungusDialogue -= ExitFungusDialogue;
        PlayerInputHandler.OnTriggerFungusDialog -= TriggerFungusDialog;
    }

    private void TriggerFungusDialog()
    {
        Fungus.CustomEventHandler fungusCustomEventHandler = FindObjectOfType<Fungus.CustomEventHandler>();
        if (fungusCustomEventHandler != null) fungusCustomEventHandler.TriggerDialog();
    }

    private void EnterFungusDialogue()
    {
        Player player = FindObjectOfType<Player>();
        player.ChangeStateToInDialogue();
    }

    private void ExitFungusDialogue()
    {
        Player player = FindObjectOfType<Player>();
        player.ResetState();
        OnExitFungusDialogue?.Invoke();
    }
}
