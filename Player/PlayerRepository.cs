using System.Collections.Generic;
using UnityEngine;

public class PlayerRepository
{
    private static string PLAYER_MAX_HEALTH_KEY() => ProfileRepository.GetCurrentProfileKey() + "PlayerMaxHealth";
    private static string PLAYER_ENABLED_ABILITIES_KEY() => ProfileRepository.GetCurrentProfileKey() + "PlayerEnabledAbilities";
    private static string PLAYER_WORLD_TOKENS_KEY() => ProfileRepository.GetCurrentProfileKey() + "WorldTokens";
    private static string PLAYER_UNLOCKED_WORLDS() => ProfileRepository.GetCurrentProfileKey() + "UnlockedWorlds";

    public static int PlayerMaxHealth
    {
        get => PlayerPrefs.GetInt(PLAYER_MAX_HEALTH_KEY());
        set => PlayerPrefs.SetInt(PLAYER_MAX_HEALTH_KEY(), value);
    }

    public static bool HasPlayerMaxHealth() => PlayerPrefs.HasKey(PLAYER_MAX_HEALTH_KEY());

    public static void SaveEnabledAbilities(List<string> enabledAbilities)
    {
        PlayerPrefs.SetString(PLAYER_ENABLED_ABILITIES_KEY(), string.Join(",", enabledAbilities.ToArray()));
        PlayerPrefs.Save();
    }

    public static List<string> GetEnabledAbilities()
    {
        return new List<string>(PlayerPrefs.GetString(PLAYER_ENABLED_ABILITIES_KEY()).Split(','));
    }

    public static int WorldTokens
    {
        get => PlayerPrefs.GetInt(PLAYER_WORLD_TOKENS_KEY());
        set => PlayerPrefs.SetInt(PLAYER_WORLD_TOKENS_KEY(), value);
    }

    public static void SaveUnlockedWorlds(List<string> unlockedWorlds)
    {
        PlayerPrefs.SetString(PLAYER_UNLOCKED_WORLDS(), string.Join(",", unlockedWorlds.ToArray()));
        PlayerPrefs.Save();
    }

    public static List<string> GetUnlockedWorlds()
    {
        return new List<string>(PlayerPrefs.GetString(PLAYER_UNLOCKED_WORLDS()).Split(','));
    }
}
