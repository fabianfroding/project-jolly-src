public class InputModeEvent
{
    private readonly EInputMode newInputMode;

    public InputModeEvent(EInputMode newInputMode)
    {
        this.newInputMode = newInputMode;
    }

    public EInputMode GetNewInputMode() => newInputMode;
}
