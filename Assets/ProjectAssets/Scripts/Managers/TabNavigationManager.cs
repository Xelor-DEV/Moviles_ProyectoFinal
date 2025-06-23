using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class TabNavigationManager : MonoBehaviour
{
    [Serializable]
    public class TabItem
    {
        public string tabName;
        public GameObject[] tabObjects;
        public Transform cameraPosition;
        public Transform robotPosition;
        public UnityEvent onTabSelected;
    }

    [Header("Settings")]
    [SerializeField] private TabItem[] tabs;
    [SerializeField] private TMP_Text tabText;

    [Header("Tween Settings")]
    [SerializeField] private float transitionDuration = 1f;
    [SerializeField] private Ease transitionEase = Ease.InOutQuad;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform robotModel;
    [SerializeField] private Renderer robotRenderer;
    [SerializeField] private RobotVisibilityHandler robotVisibility;

    [Header("Debug")]
    [SerializeField] private int currentTabIndex = -1;
    [SerializeField] private bool isTransitioning = false;

    private Tween cameraTween;

    private void Start()
    {
        robotVisibility.Initialize(robotModel, mainCamera, robotRenderer, this);

        if (tabs.Length > 0)
        {
            ChangeTab(1);
        }
    }

    public void ChangeTab(int step)
    {
        if (tabs.Length == 0 || isTransitioning) return;

        int newIndex = (currentTabIndex + step) % tabs.Length;
        
        if (newIndex < 0)
        {
            newIndex += tabs.Length;
        }

        if (newIndex != currentTabIndex)
        {
            isTransitioning = true;
            robotVisibility.ResetTeleportFlag();

            SetTabActive(currentTabIndex, false);
            currentTabIndex = newIndex;
            SetTabActive(currentTabIndex, true);

            StartCameraTransition();
        }
    }

    private void StartCameraTransition()
    {
        if (mainCamera == null || tabs[currentTabIndex].cameraPosition == null || tabs[currentTabIndex].robotPosition == null)
        {
            isTransitioning = false;
            return;
        }

        Transform target = tabs[currentTabIndex].cameraPosition;

        cameraTween = mainCamera.transform.DOMove(target.position, transitionDuration).SetEase(transitionEase).OnComplete(() => 
        {
            isTransitioning = false;
            robotVisibility.TeleportRobotIfNeeded(tabs[currentTabIndex].robotPosition);
        });
    }

    private void SetTabActive(int index, bool active)
    {
        if (index < 0 || index >= tabs.Length) return;

        for (int i = 0; i < tabs[index].tabObjects.Length; ++i)
        {
            GameObject obj = tabs[index].tabObjects[i];
            if (obj != null)
            {
                obj.SetActive(active);
            }
        }

        if (active == true)
        {
            if (tabText != null) tabText.text = tabs[index].tabName;
            tabs[index].onTabSelected?.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (cameraTween != null && cameraTween.IsActive())
        {
            cameraTween.Kill();
        }
    }

    public void OnRobotBecameInvisible()
    {
        if (isTransitioning)
        {
            robotVisibility.TeleportRobotImmediately(tabs[currentTabIndex].robotPosition);
        }
    }

    public void test()
    {
        Debug.Log("Test function called");
    }
}