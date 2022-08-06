using UnityEngine;

public class ProfileRepository
{
    public static int CurrentProfileIndex { get; private set; }

    // PROFILE_KEY + 0 or 1. 0 means not saved, 1 means saved.
    private static readonly string PROFILE_KEY = "Profile";
    private static readonly string CURRENT_SCENE_KEY = "CurrentScene";

    public static string GetCurrentProfileKey() => PROFILE_KEY + CurrentProfileIndex;

    public static void SetCurrentProfileIndex(int profileIndex)
    {
        if (profileIndex < 1) profileIndex = 1;
        else if (profileIndex > 3) profileIndex = 3;
        CurrentProfileIndex = profileIndex;
    }

    public static void InitProfiles(int numProfiles)
    {
        for (int i = 1; i <= numProfiles; i++)
        {
            if (PlayerPrefs.HasKey(PROFILE_KEY + i))
            {
                PlayerPrefs.SetInt(PROFILE_KEY + i, 1);
            }
        }
    }

    public static void CreateProfile(int profileIndex) => PlayerPrefs.SetInt(PROFILE_KEY + profileIndex, 1);
    public static bool HasProfile(int profileIndex) => PlayerPrefs.HasKey(PROFILE_KEY + profileIndex);

    public static void SaveProfileSceneName(int profileIndex, string sceneName) =>
        PlayerPrefs.SetString(PROFILE_KEY + profileIndex + CURRENT_SCENE_KEY, sceneName);

    public static string LoadProfileSceneName(int profileIndex) => 
        PlayerPrefs.GetString(PROFILE_KEY + profileIndex + CURRENT_SCENE_KEY);

    public static void DeleteAllSaveData() => PlayerPrefs.DeleteAll();
}
