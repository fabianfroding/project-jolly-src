using UnityEngine;
using UnityEngine.UI;

public class WidgetGameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;

    private void Awake()
    {
        PlayerPawn.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        PlayerPawn.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        GetComponent<Image>().enabled = true;
        if (gameOverText)
            gameOverText.SetActive(true);
    }
}
