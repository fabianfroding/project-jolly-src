using UnityEngine;

public class RevivePointRepository
{
    private static string REVIVE_POINT_GO_NAME_KEY() => "RevivePointGOName";
    private static string REVIVE_POINT_SCENE_NAME_KEY() => "RevivePointSceneName";

    public static string CurrentRevivePointGOName
    {
        get => PlayerPrefs.GetString(REVIVE_POINT_GO_NAME_KEY());
        set => PlayerPrefs.SetString(REVIVE_POINT_GO_NAME_KEY(), value);
    }

    public static string CurrentRevivePointSceneName
    {
        get => PlayerPrefs.GetString(REVIVE_POINT_SCENE_NAME_KEY());
        set => PlayerPrefs.SetString(REVIVE_POINT_SCENE_NAME_KEY(), value);
    }
}
