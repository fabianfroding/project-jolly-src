using UnityEngine;

public class LevelChangeTriggerBox : TriggerBox
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform levelEntry;

    public LevelData GetLevelData() => levelData;
    public Vector2 GetLevelEntryPoint() => levelEntry.position;

    protected override void TriggerBehavior()
    {
        EventBus.Publish(new LevelChangeEvent(levelData));
    }
}
