using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class WidgetManager : Manager<WidgetManager>
{
    [SerializeField] private GameObject pauseMenuWidget;
    [SerializeField] private GameObject gameOverlayWidget;
    
    [SerializeField] private InputActionReference BackAction;
    [SerializeField] private InputActionReference PauseAction;
    
    private List<WidgetBase> widgetPool;
    private List<WidgetBase> widgetStack;

    private float lastPopTime;

    protected override void Awake()
    {
        base.Awake();
        
        widgetPool = new List<WidgetBase>();
        widgetStack = new List<WidgetBase>();
        
        if (gameOverlayWidget)  gameOverlayWidget = Instantiate(gameOverlayWidget);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<InputActionEvent>(HandleInputActionEvent);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<InputActionEvent>(HandleInputActionEvent);
    }

    public GameObject GetWidgetGameOverlayGO() => gameOverlayWidget;
    
    public static GameObject PushWidget(GameObject pushedWidgetObject)
    {
        WidgetManager widgetManager = WidgetManager.Instance;
        if (!widgetManager)
            return null;
        
        return widgetManager.PushWidgetInternal(pushedWidgetObject);
    }
    
    public static void PopWidget(GameObject poppedWidgetObject)
    {
        WidgetManager widgetManager = Instance;
        if (!widgetManager)
            return;
        
        widgetManager.PopWidgetInternal(poppedWidgetObject);
    }

    private GameObject PushWidgetInternal(GameObject pushedWidgetObject)
    {
        WidgetBase pushedWidget = pushedWidgetObject.GetComponent<WidgetBase>();
        if (!pushedWidget)
            return null;
        
        // Disable the widget currently at the top of the stack.
        WidgetBase currentTopWidget = widgetStack.LastOrDefault();
        if (currentTopWidget)
        {
            currentTopWidget.gameObject.SetActive(false);
        }
        
        // If pushed widget exists in pool, enable it and move it to last position in stack.
        WidgetBase pooledWidget = widgetPool.Find(w => w.GetType() == pushedWidget.GetType());
        if (pooledWidget)
        {
            pooledWidget.gameObject.SetActive(true);
            widgetPool.Remove(pooledWidget);
            widgetStack.Add(pooledWidget);
            return pooledWidget.gameObject;
        }
        
        GameObject newWidgetObject = Instantiate(pushedWidgetObject);
        WidgetBase newWidget = newWidgetObject.GetComponent<WidgetBase>();
        widgetStack.Add(newWidget);
        return newWidgetObject;
    }

    private void PopWidgetInternal(GameObject poppedWidgetObject)
    {
        poppedWidgetObject.SetActive(false);
        
        WidgetBase poppedWidget = poppedWidgetObject.GetComponent<WidgetBase>();
        if (!poppedWidget)
            return;
        
        widgetStack.Remove(poppedWidget);
        widgetPool.Add(poppedWidget);
        
        poppedWidget.ClearLastSelected();

        if (widgetStack.Count > 0)
        {
            widgetStack.Last().gameObject.SetActive(true);
        }
    }

    private void PopCurrentActiveWidget()
    {
        if (widgetStack.Count == 0)
            return;
        
        WidgetBase currentActiveWidget = widgetStack.Last();
        if (!currentActiveWidget)
            return;

        if (!currentActiveWidget.gameObject.activeSelf)
            return;

        if (Time.unscaledTime <= lastPopTime + 0.1f)
            return;
        lastPopTime = Time.unscaledTime;
        
        currentActiveWidget.Pop();
    }

    private void HandleInputActionEvent(InputActionEvent inputActionEvent)
    {
        if (inputActionEvent.GetInputAction() == BackAction.action)
        {
            WidgetBase currentActiveWidget = widgetStack.Last();
            if (currentActiveWidget)
            {
                if (currentActiveWidget.AllowsBackHandling())
                    PopCurrentActiveWidget();
            }
        }

        if (inputActionEvent.GetInputAction() == PauseAction.action)
        {
            PushWidgetInternal(pauseMenuWidget);
        }
    }
}
