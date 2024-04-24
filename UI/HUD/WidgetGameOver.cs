using UnityEngine;
using UnityEngine.UI;

public class WidgetGameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;

    private void Awake()
    {
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnRevivePlayer += OnRevivePlayer;
    }

    private void OnDestroy()
    {
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnRevivePlayer -= OnRevivePlayer;
    }

    private void OnGameOver()
    {
        GetComponent<Image>().enabled = true;
        if (gameOverText)
            gameOverText.SetActive(true);
    }

    private void OnRevivePlayer()
    {
        GetComponent<Image>().enabled = false;
        if (gameOverText)
            gameOverText.SetActive(false);
    }
}
