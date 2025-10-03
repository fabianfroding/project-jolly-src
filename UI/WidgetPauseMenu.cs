using UnityEngine;
using UnityEngine.SceneManagement;

public class WidgetPauseMenu : WidgetBase
{
    [SerializeField] private string quitConfirmPromptTitle = "Quit to Menu?";

    private WidgetConfirmPrompt quitConfirmPrompt;

    protected override void OnEnable()
    {
        base.OnEnable();
        Time.timeScale = 0f;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
    
    public void HandleContinue()
    {
        Pop();
    }

    public void HandleQuit(GameObject confirmPromptObject)
    {
        if (confirmPromptObject)
        {
            GameObject pushedWidgetObject = WidgetManager.PushWidget(confirmPromptObject);
            if (pushedWidgetObject)
            {
                quitConfirmPrompt = pushedWidgetObject.GetComponent<WidgetConfirmPrompt>();
                if (quitConfirmPrompt)
                {
                    quitConfirmPrompt.SetTitle(quitConfirmPromptTitle);
                    quitConfirmPrompt.OnConfirm += ConfirmQuit;
                }
            }
        }
        else
        {
            ConfirmQuit(true);
        }
    }

    protected override void Deactivate()
    {
        Time.timeScale = 1f;
        base.Deactivate();
    }

    private void ConfirmQuit(bool confirmed)
    {
        if (quitConfirmPrompt)
        {
            quitConfirmPrompt.OnConfirm -= ConfirmQuit;
        }
        
        if (confirmed)
        {
            GameInstance.Instance.GoToMainMenu();
        }
    }
}
