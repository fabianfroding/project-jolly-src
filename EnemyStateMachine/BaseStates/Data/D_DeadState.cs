using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/StateData/DeadState")]
public class D_DeadState : ScriptableObject
{
    public GameObject deathVFXPrefab;
    public AudioClip deathAudioClip;

    [Tooltip("Leave empty if no corpse should spawn.")]
    public GameObject corpsePrefab;

    [Tooltip("Leave empty if no currency should drop.")]
    public GameObject currencyDropPrefab;
}
