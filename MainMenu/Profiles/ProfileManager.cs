using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private string newGameSceneName = "TwistedWoods1";
    [SerializeField] private int numProfiles = 3;

    [Tooltip("Check to delete all player prefs data.")]
    [SerializeField] private bool clearPlayerPrefs = false;

    private static ProfileManager instance;
    public static ProfileManager Instance {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ProfileManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        if (clearPlayerPrefs) ProfileRepository.DeleteAllSaveData();
        InitProfiles();
    }

    private static void CreateProfile(int profileIndex) => ProfileRepository.CreateProfile(profileIndex);
    public static bool HasProfile(int profileIndex) => ProfileRepository.HasProfile(profileIndex);

    private static void SaveCurrentProfileCurrentSceneName(string sceneName) =>
        ProfileRepository.SaveProfileSceneName(ProfileRepository.CurrentProfileIndex, sceneName);

    public static void LoadSceneForCurrentProfile() =>
        SceneManager.LoadScene(ProfileRepository.LoadProfileSceneName(ProfileRepository.CurrentProfileIndex));

    public void SetCurrentProfileIndex(int profileIndex) => ProfileRepository.SetCurrentProfileIndex(profileIndex);

    public void InitProfiles() => ProfileRepository.InitProfiles(numProfiles);

    public void StartGameForProfile(int profileIndex)
    {
        SetCurrentProfileIndex(profileIndex);
        if (!HasProfile(profileIndex))
        {
            CreateProfile(profileIndex);
            SaveCurrentProfileCurrentSceneName(newGameSceneName);
        }

        SceneManager.LoadScene(LoadGameSceneScript.LOAD_GAME_SCENE_NAME);
    }
}
