using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    private LevelChangeTriggerBox[] levelChangeTriggerBoxes;

    public LevelData GetLevelData() => levelData;

    public void InitializePlayerAtEntryPoint(GameObject player, LevelData previousLevelData)
    {
        levelChangeTriggerBoxes = GetComponentsInChildren<LevelChangeTriggerBox>();
        if (previousLevelData == null) return;

        foreach (LevelChangeTriggerBox levelChangeTriggerBox in levelChangeTriggerBoxes)
        {
            if (levelChangeTriggerBox.GetLevelData() != previousLevelData) continue;

            player.transform.position = levelChangeTriggerBox.GetLevelEntryPoint();
            return;
        }
    }
}
