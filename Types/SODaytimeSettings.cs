using UnityEngine;

[CreateAssetMenu(fileName = "newReferenceVariable", menuName = "ReferenceVariables/ReferenceVariable/SODaytimeSettings")]
public class SODaytimeSettings : ScriptableObject
{
    // TODO: Consider moving currentHour and currentMinute here.
    public float dawnStartTime;
    public float dawnEndTime;
    public float duskStartTime;
    public float duskEndTime;

    public float GetDawnMidTime() => dawnStartTime + ((dawnEndTime - dawnStartTime) / 2f);
    public float GetDuskMidTime() => duskStartTime + ((duskEndTime - duskStartTime) / 2f);
}
