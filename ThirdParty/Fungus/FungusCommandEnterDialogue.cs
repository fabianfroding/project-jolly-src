using Fungus;
using System;

[CommandInfo("CustomCommand", "EnterDialogue", "")]
public class FungusCommandEnterDialogue : Command
{
    public static Action OnEnterFungusDialogue;

    public override void OnEnter()
    {
        base.OnEnter();
        Continue();
        OnEnterFungusDialogue?.Invoke();
    }
}
