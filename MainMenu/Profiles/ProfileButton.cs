using TMPro;
using UnityEngine;

public class ProfileButton : MonoBehaviour
{
    [SerializeField] private int profileIndex;
    [SerializeField] private TextMeshProUGUI textMesh;
    private static readonly string NEW_GAME_TEXT = "New Game";
    private static readonly string LOAD_GAME_TEXT = "Continue";

    private void Start()
    {
        SetText();
    }

    private void OnEnable()
    {
        SetText();
    }

    public void Clicked()
    {
        ProfileManager.Instance.StartGameForProfile(profileIndex);
    }

    private void SetText()
    {
        textMesh.text = profileIndex + ". " + 
            (ProfileManager.HasProfile(profileIndex) ? LOAD_GAME_TEXT : NEW_GAME_TEXT);
    }
}
