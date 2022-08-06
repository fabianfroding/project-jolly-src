using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/StateData/ChargeState")]
public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 6f;
    public float chargeTime = 2f;
    public float chargeCooldown = 2f;
}
