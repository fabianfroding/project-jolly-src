using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// All enemies in the scene should have unique names for this to work.

public class EnemyRepository : MonoBehaviour
{
    private static List<string> killedEnemies;

    private static string ENEMY_KEY(string enemyName)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string key = ProfileRepository.GetCurrentProfileKey() + sceneName + enemyName;
        return key;
    }

    public static bool HasBeenKilled(string enemyName)
    {
        bool hasKey = PlayerPrefs.HasKey(ENEMY_KEY(enemyName));
        bool inKilledEnemies = killedEnemies != null && killedEnemies.Contains(enemyName);

        return hasKey || inKilledEnemies;
    }

    public static void AddToKilledEnemies(string enemyName, bool neverRespawn)
    {
        if (neverRespawn)
        {
            PlayerPrefs.SetInt(ENEMY_KEY(enemyName), 1);
        }
        else
        {
            if (killedEnemies == null)
            {
                killedEnemies = new List<string>();
            }

            if (!killedEnemies.Contains(enemyName))
            {
                killedEnemies.Add(enemyName);
            }
        }
    }

    public static void ResetKilledEnemies()
    {
        killedEnemies = null;
    }
}
