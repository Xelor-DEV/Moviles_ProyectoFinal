using UnityEngine;

public class InputManager : NonPersistentSingleton<InputManager>
{
    [Header("Controllers")]
    [SerializeField] private RepairSystem repairSystemScript;

    [Header("References")]
    [SerializeField] private TouchInputHandler touchInputHandler;

    [Header("Active Controller")]
    [SerializeField] private bool repairSystem;

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
}