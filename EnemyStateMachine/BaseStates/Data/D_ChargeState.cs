using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/StateData/ChargeState")]
public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 6f;
    public float chargeDuration = 2f;
    public float chargeCooldown = 2f;
    public float chargeUpTime = 1f;

    public AudioClip chargeStartAudioClip;
}
