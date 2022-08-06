using UnityEngine;

public class ScreenFade : MonoBehaviour 
{
    public static readonly string SCREEN_FADE_IN_ANIMATION_NAME = "ScreenFadeIn";
    public static readonly string SCREEN_FADE_OUT_ANIMATION_NAME = "ScreenFadeOut";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn() => animator.Play(SCREEN_FADE_IN_ANIMATION_NAME);
    public void FadeOut() => animator.Play(SCREEN_FADE_OUT_ANIMATION_NAME);

    public float GetFadeOutDuration() => GetAnimationDuration(SCREEN_FADE_OUT_ANIMATION_NAME);

    private float GetAnimationDuration(string animationName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
                return clip.length;
        }
        return 0f;
    }
}
