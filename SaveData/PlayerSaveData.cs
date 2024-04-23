using System;

[Serializable]
public class PlayerSaveData
{
    public float[] position;
    public int playerHealth;
    public int playerMaxHealth;
    public string sceneName;

    public PlayerSaveData(PlayerPawn playerPawn, float xPos, float yPos, string sceneName)
    {
        position = new float[2];
        position[0] = xPos;
        position[1] = yPos;

        playerHealth = playerPawn.HealthComponent.CurrentHealth;
        playerMaxHealth = playerPawn.HealthComponent.GetMaxHealth();

        this.sceneName = sceneName;
    }
}
