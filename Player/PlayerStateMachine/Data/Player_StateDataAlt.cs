using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/PlayerData/StateDataAlt")]
public class Player_StateDataAlt : Player_StateDataBase
{
    [Header("Alt Form")]
    public MutableFloat altFormDuration;
    public AudioClip[] altFormFootstepAudioClip;

    public CameraShakeEvent altFormJumpLandCameraShakeEvent;
    public CameraShakeEvent altFormMoveCameraShakeEvent;
}
