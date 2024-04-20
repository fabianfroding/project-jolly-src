using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/StateData/PlayerDetectedState")]
public class D_PlayerDetectedState : ScriptableObject
{
    [Header("Aggro")]
    public GameObject aggroSoundPrefab;
    [Tooltip("The time it takes for the enemy to re-use the aggro sound if the enemy re-aquires the player target.")]
    public float aggroSoundResetTime = 5f;

    public float closeRangeActionTime = 1f;
    public float longRangeActionTime = 1.5f;
}
