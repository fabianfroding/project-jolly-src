using UnityEngine;

public class EnemyListRepository
{
    private static string ENEMY_LIST_NUM_KILLED_KEY(string enemyName) =>
        ProfileRepository.GetCurrentProfileKey() + enemyName.Replace(" ", "") + "EnemyListNumKilled";

    public static void AddEnemyListNumKilled(string enemyName)
    {
        string key = ENEMY_LIST_NUM_KILLED_KEY(enemyName);
        PlayerPrefs.SetInt(key, PlayerPrefs.HasKey(key) ? LoadEnemyListNumKilled(enemyName) + 1 : 1);

        // If enemy is shiny, call this function recursively to register the kill to the kills of the non-shiny version.
        if (enemyName.Contains(ShinyEnemyRandomizer.SHINY_ENEMY_NAME_PREFIX))
        {
            AddEnemyListNumKilled(enemyName.Replace(ShinyEnemyRandomizer.SHINY_ENEMY_NAME_PREFIX, ""));
        }
    }

    public static int LoadEnemyListNumKilled(string enemyName)
    {
        string key = ENEMY_LIST_NUM_KILLED_KEY(enemyName);
        if (PlayerPrefs.HasKey(key))
        {
            int kills = PlayerPrefs.GetInt(key);
            return kills;
        }
        return 0;
    }
}
