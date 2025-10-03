using System;

[Serializable]
public class PlayerSaveData
{
    public float[] position;
    public int playerHealth;
    public int playerMaxHealth;
    public string sceneName;

    public int currentHour;
    public int currentMinute;

    public PlayerSaveData(PlayerCharacter playerCharacter, float xPos, float yPos, string sceneName)
    {
        position = new float[2];
        position[0] = xPos;
        position[1] = yPos;

        playerHealth = playerCharacter.HealthComponent.GetCurrenthealth();
        playerMaxHealth = playerCharacter.HealthComponent.GetMaxHealth();

        this.sceneName = sceneName;
    }
}
