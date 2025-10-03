using UnityEngine;

public class WidgetGameEnd : WidgetBase
{
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
        animator.Play(closeAnim.name);
    }

    protected override void Deactivate()
    {
        Time.timeScale = 1f;
        base.Deactivate();
        GameInstance.Instance.GoToMainMenu();
    }
}
