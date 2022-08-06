using UnityEngine;

public class PlayerRepository
{
    private static string PLAYER_MAX_HEALTH_KEY() => ProfileRepository.GetCurrentProfileKey() + "PlayerMaxHealth";

    public static int PlayerMaxHealth
    {
        get => PlayerPrefs.GetInt(PLAYER_MAX_HEALTH_KEY());
        set => PlayerPrefs.SetInt(PLAYER_MAX_HEALTH_KEY(), value);
    }

    public static bool HasPlayerMaxHealth() => PlayerPrefs.HasKey(PLAYER_MAX_HEALTH_KEY());
}
