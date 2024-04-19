using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/StateData/DodgeState")]
public class D_DodgeState : ScriptableObject
{
    public float dodgeSpeed = 10f;
    public float dodgeTime = 0.2f;
    public float dodgeCooldown = 2f;
    public Vector2 dodgeAngle;
}
