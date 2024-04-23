using System;

[Serializable]
public class PlayerSaveData
{
    public float[] position;
    public int playerHealth;
    public int playerMaxHealth;
    public string sceneName;

    public PlayerSaveData(PlayerPawn playerPawn, string sceneName)
    {
        position = new float[3];
        position[0] = playerPawn.transform.position.x;
        position[1] = playerPawn.transform.position.y;
        position[2] = playerPawn.transform.position.z;

        playerHealth = playerPawn.HealthComponent.CurrentHealth;
        playerMaxHealth = playerPawn.HealthComponent.GetMaxHealth();

        this.sceneName = sceneName;
    }
}
