using Fungus;
using System;

[CommandInfo("CustomCommand", "ExitDialogue", "")]
public class FungusCommandExitDialogue : Command
{
    public static Action OnExitFungusDialogue;

    public override void OnEnter()
    {
        base.OnEnter();
        Continue();
        OnExitFungusDialogue?.Invoke();
    }
}
