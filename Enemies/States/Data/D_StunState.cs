using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/StateData/StunState")]
public class D_StunState : ScriptableObject
{
    public float stunTime = 3f;
    public float stunKnockbackTime = 0.2f;
    public float stunKnockbackSpeed = 2f;
    public Vector2 stunKnockbackAngle;
}
