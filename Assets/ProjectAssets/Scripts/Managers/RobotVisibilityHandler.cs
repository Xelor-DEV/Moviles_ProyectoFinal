using UnityEngine;

public class RobotVisibilityHandler : MonoBehaviour
{
    private Renderer robotRenderer;
    private Transform robotModel;
    private Camera mainCamera;
    private TabNavigationManager tabNavigationManager;
    private bool needsTeleport;

    public void Initialize(Transform robot, Camera cam, Renderer renderer, TabNavigationManager tabNavigation)
    {
        robotModel = robot;
        mainCamera = cam;
        robotRenderer = renderer;
        tabNavigationManager = tabNavigation;
    }

    public void ResetTeleportFlag()
    {
        needsTeleport = true;
    }

    public void TeleportRobotImmediately(Transform target)
    {
        if (robotModel != null && needsTeleport)
        {
            robotModel.position = target.position;
            robotModel.rotation = target.rotation;
            needsTeleport = false;
        }
    }

    public void TeleportRobotIfNeeded(Transform target)
    {
        if (needsTeleport && robotModel != null)
        {
            robotModel.position = target.position;
            robotModel.rotation = target.rotation;
        }
        needsTeleport = false;
    }

  
    private void OnBecameInvisible()
    {
        if (robotRenderer != null && robotRenderer.isVisible == false)
        {
            tabNavigationManager.OnRobotBecameInvisible();
        }
    }
}