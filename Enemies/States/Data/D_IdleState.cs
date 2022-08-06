using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/StateData/IdleState")]
public class D_IdleState : ScriptableObject
{
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;
}
