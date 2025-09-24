using UnityEngine;
using UnityEngine.EventSystems;

public class WidgetBase : MonoBehaviour
{
    [SerializeField] private GameObject defaultSelected;
    [SerializeField] private EInputMode activatedInputMode = EInputMode.UI;
    [SerializeField] private EInputMode deactivatedInputMode = EInputMode.Gameplay;

    [SerializeField] private AnimationClip openAnim;
    [SerializeField] protected AnimationClip closeAnim;
    
    private GameObject lastSelected;
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (!animator)
        {
            SelectDefaultElement();
        }
    }
    
    protected virtual void OnEnable()
    {
        InputModeEvent inputModeEvent = new(activatedInputMode);
        EventBus.Publish(inputModeEvent);
        
        if (animator)
        {
            animator.Play(openAnim.name);
        }
        else
        {
            Activate();
        }
    }

    protected virtual void OnDisable()
    {
        if (EventSystem.current)
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        
        InputModeEvent inputModeEvent = new(deactivatedInputMode);
        EventBus.Publish(inputModeEvent);
        
        EventBus.Unsubscribe<InputTypeEvent>(HandleInputTypeEvent);
    }
    
    public virtual void Pop()
    {
        if (animator)
        {
            animator.Play(closeAnim.name);
        }
        else
        {
            Deactivate();
        }
    }
    
    public void ClearLastSelected()
    {
        lastSelected = null;
    }

    public void OpenAnimationDone()
    {
        Activate();
    }

    public virtual void CloseAnimationDone()
    {
        Deactivate();
    }
    
    protected virtual void Activate()
    {
        EventBus.Subscribe<InputTypeEvent>(HandleInputTypeEvent);
        
        RestoreSelection();
    }

    protected virtual void Deactivate()
    {
        WidgetManager.PopWidget(gameObject);
    }

    private void HandleInputTypeEvent(InputTypeEvent inputTypeEvent)
    {
        if (inputTypeEvent.newInputType == EInputType.Gamepad)
        {
            SelectDefaultElement();
        }
        else if (inputTypeEvent.newInputType == EInputType.KeyboardAndMouse)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void SelectDefaultElement()
    {
        if (EventSystem.current && defaultSelected)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelected);
        }
    }
    
    private void RestoreSelection()
    {
        if (EventSystem.current)
        {
            if (lastSelected)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
            else if (defaultSelected)
            {
                EventSystem.current.SetSelectedGameObject(defaultSelected);
            }
        }
    }
}
