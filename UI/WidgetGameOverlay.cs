using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WidgetGameOverlay : WidgetBase
{
    [SerializeField] protected AnimationClip fadeInFastAnim;
    
    [SerializeField] private Sprite healthFillImage;
    [SerializeField] private Sprite healthEmptyImage;
    
    [SerializeField] private List<Image> healthImages;

    private bool inFadeAnim = false;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<HealthCombatEvent>(HandleHealthCombatEvent);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<HealthCombatEvent>(HandleHealthCombatEvent);
    }

    public override void OpenAnimationDone()
    {
        base.OpenAnimationDone();
        inFadeAnim = false;
    }

    public override void CloseAnimationDone()
    {
        if (inFadeAnim)
            return;
        
        // TEMP. Notify GameInstance instead.
        SceneManager.LoadScene("MainMenuScene");
    }
    
    public async Task FadeOut()
    {
        if (closeAnim)
        {
            inFadeAnim = true;
            animator.Play(closeAnim.name);
            await Task.Delay((int)(closeAnim.length * 1000));
        }
    }

    public void FadeIn()
    {
        if (openAnim)
        {
            animator.Play(openAnim.name);
        }
    }

    public void FadeInFast()
    {
        if (fadeInFastAnim)
        {
            animator.Play(fadeInFastAnim.name);
        }
    }

    private void HandleHealthCombatEvent(HealthCombatEvent healthCombatEvent)
    {
        PlayerCharacter playerCharacter = healthCombatEvent.GetDamagedObject().GetComponent<PlayerCharacter>();
        if (!playerCharacter)
            return;

        int newHealth = healthCombatEvent.GetNewHealth();
        for (int i = 0; i < healthImages.Count; i++)
        {
            healthImages[i].sprite = i <= newHealth - 1 ? healthFillImage : healthEmptyImage;
        }
        
        if (newHealth <= 0)
            animator.Play(closeAnim.name);
    }
}
