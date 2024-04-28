using UnityEngine;

[CreateAssetMenu(fileName = "newReferenceVariable", menuName = "ReferenceVariables/ReferenceVariable/SOInteractionData")]
public class SOInteractionData : ScriptableObject
{
    [HideInInspector] public IInteractable interactable; // Exposing interface variable in inspector causes error spam.
}
