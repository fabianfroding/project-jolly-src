using UnityEngine;

[CreateAssetMenu(fileName = "newBlockStateData", menuName = "Data/StateData/BlockState")]
public class D_BlockState : ScriptableObject
{
    [Range(0f, 1f)] public float chanceToCounterOnBlock = 0f;
    public GameObject sfxBlockPrefab;
}
