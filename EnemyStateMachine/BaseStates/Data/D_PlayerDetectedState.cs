using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerDetectedStateData", menuName = "Data/StateData/PlayerDetectedState")]
public class D_PlayerDetectedState : ScriptableObject
{
    public GameObject aggroSoundPrefab;
    public float closeRangeActionTime = 1f;
    public float longRangeActionTime = 1.5f;
}
