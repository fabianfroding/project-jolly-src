using UnityEngine;

public class LevelChangeEvent
{
    private readonly LevelData levelDataToLoad;

    public LevelChangeEvent(LevelData levelDataToLoad)
    {
        this.levelDataToLoad = levelDataToLoad;
    }

    public LevelData GetLevelDataToLoad() => levelDataToLoad;
}
