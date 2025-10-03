using UnityEngine;

public class EncounterTriggerBox : TriggerBox
{
    [SerializeField] private EnemyCharacter enemyToDefeat;
    [SerializeField] private GameObject[] objectsToDisable;

    private AudioSource encounterMusicAudioSource;

    private void Awake()
    {
        encounterMusicAudioSource = GetComponent<AudioSource>();
    }
    
    protected override void TriggerBehavior()
    {
        if (!enemyToDefeat)
            return;
        
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        encounterMusicAudioSource.Play();

        enemyToDefeat.EnemyDefeated += HandleEnemyDefeated;
    }

    private void HandleEnemyDefeated()
    {
        enemyToDefeat.EnemyDefeated -= HandleEnemyDefeated;

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(true);
        }

        encounterMusicAudioSource.Stop();
    }
}
