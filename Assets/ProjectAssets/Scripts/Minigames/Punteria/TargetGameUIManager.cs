using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TargetGameUIManager : NonPersistentSingleton<TargetGameUIManager>
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI hitsText;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalCoinsText;
    [SerializeField] private TextMeshProUGUI finalHitsText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    [SerializeField] private string gameScene;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private RobotNeeds robotNeeds;

    [SerializeField] private float funAdded = 0.25f;
    private int currentPrismites;
    private int hitCount;

    private void Start()
    {
        currentPrismites = 0;
        hitCount = 0;

        UpdateUI();
        gameOverPanel.SetActive(false);
        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    public void AddHit()
    {
        hitCount++;
        currentPrismites += 1;
        UpdateUI();
    }

    void UpdateUI()
    {
        coinText.text = $"Prismites: {currentPrismites}";
        hitsText.text = $"Successes: {hitCount}";
    }

    public void ShowGameOverPanel()
    {
        if (resourceManager != null)
        {
            resourceManager.AddPrismites(currentPrismites);
        }

        finalCoinsText.text = $"Prismites collected: {currentPrismites}";
        finalHitsText.text = $"Successes achieved: {hitCount}";
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMenu()
    {
        Time.timeScale = 1;
        robotNeeds.Fun = Mathf.Clamp01(robotNeeds.Fun + funAdded);
        StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(gameScene));
    }
}