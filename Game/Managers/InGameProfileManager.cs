using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameProfileManager : MonoBehaviour
{
    private static InGameProfileManager instance;
    public static InGameProfileManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<InGameProfileManager>();
            return instance;
        }
    }

    private void Awake()
    {
        RevivePoint.OnActivateRevivePoint += SaveCurrentProfileCurrentSceneName;
    }

    private void OnDestroy()
    {
        RevivePoint.OnActivateRevivePoint -= SaveCurrentProfileCurrentSceneName;
    }

    private void SaveCurrentProfileCurrentSceneName() => 
        ProfileRepository.SaveProfileSceneName(ProfileRepository.CurrentProfileIndex, SceneManager.GetActiveScene().name);
}
