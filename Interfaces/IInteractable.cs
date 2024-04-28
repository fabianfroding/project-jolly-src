public interface IInteractable
{
    public bool AdvanceInteraction();
    public void Interact();
    public void InteractEnd();
    public bool IsInteractable();
    public string GetInteractionText();
}
