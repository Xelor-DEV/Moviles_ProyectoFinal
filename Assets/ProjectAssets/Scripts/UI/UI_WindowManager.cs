using UnityEngine;

public class UI_WindowManager : MonoBehaviour
{
    [Header("Window Settings")]
    [SerializeField] private GameObject[] windows;
    private int activeWindowIndex = -1;

    public int ActiveWindowIndex
    {
        get
        {
            return activeWindowIndex;
        }
    }

    private void Start()
    {
        DeactivateAllWindows();
    }

    private void DeactivateAllWindows()
    {
        for (int i = 0; i < windows.Length; ++i)
        {
            if (windows[i] != null)
            {
                windows[i].SetActive(false);
            }
        }

        activeWindowIndex = -1;
    }

    public void ActivateWindow(int windowIndex)
    {
        if (windowIndex == activeWindowIndex || activeWindowIndex != -1) return;

        if (IsValidWindowIndex(activeWindowIndex))
        {
            windows[activeWindowIndex].SetActive(false);
        }

        if (IsValidWindowIndex(windowIndex))
        {
            windows[windowIndex].SetActive(true);
            activeWindowIndex = windowIndex;
        }
        else
        {
            Debug.LogWarning("Invalid window index: " + windowIndex);
            activeWindowIndex = -1;
        }
    }

    public void CloseActiveWindow()
    {
        if (IsValidWindowIndex(activeWindowIndex))
        {
            windows[activeWindowIndex].SetActive(false);
            activeWindowIndex = -1;
        }
    }

    private bool IsValidWindowIndex(int index)
    {
        if (index >= 0 && index < windows.Length && windows[index] != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}