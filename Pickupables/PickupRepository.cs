using UnityEngine;
using UnityEngine.SceneManagement;

// All pickups in the scene should have unique names for this to work.

public class PickupRepository : MonoBehaviour
{
    private static string PICKUP_KEY(string pickupName)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string key = ProfileRepository.GetCurrentProfileKey() + sceneName + pickupName;
        return key;
    }

    // A value of <= 0 corresponds to not having been picked up.
    // A value of >= 1 corresponds to having been picked up.
    public static void SetHasBeenPickedUp(string pickupName, bool hasBeenPickedUp)
    {
        int state = hasBeenPickedUp ? 1 : 0;
        PlayerPrefs.SetInt(PICKUP_KEY(pickupName), state);
    }

    public static bool GetHasBeenPickedUp(string pickupName)
    {
        bool hasKey = PlayerPrefs.HasKey(PICKUP_KEY(pickupName));
        bool pickedUpState = PlayerPrefs.GetInt(PICKUP_KEY(pickupName)) >= 1;
        return hasKey && pickedUpState;
    }
}
