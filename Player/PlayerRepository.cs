using System.Collections.Generic;
using UnityEngine;

public class PlayerRepository
{
    private static string PLAYER_MAX_HEALTH_KEY() => ProfileRepository.GetCurrentProfileKey() + "PlayerMaxHealth";
    private static string PLAYER_ENABLED_ABILITIES_KEY() => ProfileRepository.GetCurrentProfileKey() + "PlayerEnabledAbilities";

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
}
