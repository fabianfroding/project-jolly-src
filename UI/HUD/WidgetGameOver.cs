using UnityEngine;
using UnityEngine.UI;

public class WidgetGameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;

    private void Awake()
    {
        Player.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        Player.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        GetComponent<Image>().enabled = true;
        if (gameOverText)
            gameOverText.SetActive(true);
    }
}
