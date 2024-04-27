using UnityEngine;

[CreateAssetMenu(fileName = "newReferenceVariable", menuName = "ReferenceVariables/ReferenceVariable/SODaytimeSettings")]
public class SODaytimeSettings : ScriptableObject
{
    public int currentHour;
    public int currentMinute;
    [SerializeField] private float dawnStartTime;
    [SerializeField] private float dawnEndTime;
    [SerializeField] private float duskStartTime;
    [SerializeField] private float duskEndTime;

    public float DawnStartTime => dawnStartTime;
    public float DawnEndTime => dawnEndTime;
    public float DuskStartTime => duskStartTime;
    public float DuskEndTime => duskEndTime;

    public float DawnMidTime => dawnStartTime + ((dawnEndTime - dawnStartTime) / 2f);
    public float DuskMidTime => duskStartTime + ((duskEndTime - duskStartTime) / 2f);
}
