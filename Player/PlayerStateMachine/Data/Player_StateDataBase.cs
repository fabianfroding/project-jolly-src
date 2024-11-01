using UnityEngine;

public class Player_StateDataBase : ScriptableObject
{
    [Header("Attack State")]
    public Types.DamageData attackDamageData;

    [Header("Jump State")]
    public float jumpVelocity = 25f;
    public GameObject jumpVFXPrefab;
    public GameObject jumpTrailVFXPrefab;
    public AudioClip jumpAudioClip;
    public AudioClip jumpLandAudioClip;
}
