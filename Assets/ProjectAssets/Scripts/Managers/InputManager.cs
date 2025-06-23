using UnityEngine;

public class InputManager : NonPersistentSingleton<InputManager>
{
    [Header("Controllers")]
    [SerializeField] private RepairSystem repairSystemScript;
    [SerializeField] private TabNavigationManager tabNavigationManagerScript;

    [Header("References")]
    [SerializeField] private TouchInputHandler touchInputHandler;
    [SerializeField] private UI_WindowManager uiWindowManager;


    [Header("Active Controller")]
    [SerializeField] private bool repairSystem;
    [SerializeField] private bool tabNavigationManager;

    public bool RepairSystem
    {
        set
        {
            repairSystem = value;
        }
    }

    public bool TabNavigationManager
    {
        set
        {
            tabNavigationManager = value;
        }
    }


    public void SelectonDragStartWithPosition(Vector3 position)
    {
        if (repairSystem)
        {
            repairSystemScript.OnDragStart(position);
        }
    }

    public void SelectorDragWithPosition(Vector3 position)
    {
        if (repairSystem)
        {
            repairSystemScript.OnDrag(position);
        }
    }

    public void SelectorDragEnd()
    {
        if (repairSystem)
        {
            repairSystemScript.OnDragEnd();
        }
    }

    public void SelectorSwipeLeft()
    {
        if (tabNavigationManager && uiWindowManager.ActiveWindowIndex == -1)
        {
            tabNavigationManagerScript.ChangeTab(1);
        }
    }

    public void SelectorSwipeRight()
    {
        if (tabNavigationManager && uiWindowManager.ActiveWindowIndex == -1)
        {
            tabNavigationManagerScript.ChangeTab(-1);
        }
    }

    public void DeactivateAllActiveControllers()
    {
        if (repairSystem)
        {
            repairSystem = false;
        }
        else if (tabNavigationManager)
        {
            tabNavigationManager = false;
        }
    }
}