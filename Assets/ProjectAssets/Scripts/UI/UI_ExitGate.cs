using UnityEngine;

public class UI_ExitGate : MonoBehaviour
{
    [Header("Window Settings")]
    [SerializeField] private GameObject loadingWindow;

    private void Start()
    {
        if (loadingWindow != null)
        {
            loadingWindow.SetActive(false);
        }

        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    public void LoadMiniGame(string sceneName)
    {
        if (loadingWindow != null)
        {
            loadingWindow.SetActive(true);
        }
        InputManager.Instance.DeactivateAllActiveControllers();
        StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(sceneName));
    }
}