using UnityEngine;

[CreateAssetMenu(fileName = "newInteractData", menuName = "Data/Interaction/InteractData")]
public class D_InteractData : ScriptableObject
{
    [TextArea] public string[] InteractText;
}
