using UnityEngine;

public class UI_ExitGate : MonoBehaviour
{
    [Header("Window Settings")]
    [SerializeField] private GameObject[] windows;
    [SerializeField] private GameObject loadingWindow;

    private int activeWindowIndex = -1;

    private void Start()
    {

        DeactivateAllWindows();

        if (loadingWindow != null)
        {
            loadingWindow.SetActive(false);
        }
    }

    private void DeactivateAllWindows()
    {
        for(int i = 0; i < windows.Length; ++i)
        {
            if(windows[i] != null)
            {
                windows[i].SetActive(false);
            }
        }

        activeWindowIndex = -1;
    }

    public void ActivateWindow(int windowIndex)
    {
        if (activeWindowIndex >= 0 && activeWindowIndex < windows.Length && windows[activeWindowIndex] != null)
        {
            windows[activeWindowIndex].SetActive(false);
        }

        if (windowIndex >= 0 && windowIndex < windows.Length && windows[windowIndex] != null)
        {
            windows[windowIndex].SetActive(true);
            activeWindowIndex = windowIndex;
        }
        else
        {
            Debug.LogWarning("Invalid window index: " + windowIndex);
        }

        activeWindowIndex = windowIndex;
    }

    public void CloseActiveWindow()
    {
        if (activeWindowIndex >= 0 && activeWindowIndex < windows.Length && windows[activeWindowIndex] != null)
        {
            windows[activeWindowIndex].SetActive(false);
            activeWindowIndex = -1;
        }
    }

    public void LoadMiniGame(string sceneName)
    {
        if (loadingWindow != null)
        {
            loadingWindow.SetActive(true);
        }

        StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(sceneName));
    }
}