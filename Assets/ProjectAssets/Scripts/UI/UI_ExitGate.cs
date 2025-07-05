using UnityEngine;
using TMPro;

public class UI_ExitGate : MonoBehaviour
{
    [Header("Window Settings")]
    [SerializeField] private GameObject loadingWindow;

    [Header("Power Warning Settings")]
    [SerializeField] private TMP_Text[] powerWarningTexts;
    [SerializeField] private RobotNeeds needsConfig;

    [Header("References")]
    [SerializeField] private RobotStatsManager robotStatsManager;
    [SerializeField] private UI_PowerStation ui_PowerStation;

    private void OnEnable()
    {
        robotStatsManager.OnPowerUpdate.AddListener(UpdatePowerWarning);
        ui_PowerStation.OnPowerUpdate.AddListener(UpdatePowerWarning);
    }

    private void OnDisable()
    {
        robotStatsManager.OnPowerUpdate.RemoveListener(UpdatePowerWarning);
        ui_PowerStation.OnPowerUpdate.RemoveListener(UpdatePowerWarning);
    }

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

        UpdatePowerWarning();
    }

    public void UpdatePowerWarning()
    {
        string warningText = "";
        if (needsConfig.Power < needsConfig.Critical)
        {
            warningText = "Insufficient Power!";
        }
        else
        {
            warningText = "";
        }

        for (int i = 0; i < powerWarningTexts.Length; ++i)
        {
            if (powerWarningTexts[i] != null)
            {
                powerWarningTexts[i].text = warningText;
            }
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

    public void CheckPowerAndLoadMiniGame(string sceneName)
    {
        if (needsConfig.Power < needsConfig.Critical)
        {
            UpdatePowerWarning();
            return;
        }

        if (loadingWindow != null)
        {
            loadingWindow.SetActive(true);
        }

        InputManager.Instance.DeactivateAllActiveControllers();
        StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(sceneName));
    }
}