using UnityEngine;

[CreateAssetMenu(fileName = "newWorldData", menuName = "Data/WorldData/WorldData")]
public class WorldData : ScriptableObject
{
    public string worldName;
    public Types.World world;
    public AudioClip ambienceAudioClip;
    public int numWorldTokensRequired;
}
