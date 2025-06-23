using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RunnerManager : NonPersistentSingleton<RunnerManager>
{
    [Header("Referencias")]
    public ScrashCounter counterData;

    [Header("UI")]
    public TextMeshProUGUI pickupText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timeText;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalCoinsText;
    [SerializeField] private TextMeshProUGUI finalPickupsText;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private string gameScene;

    private float survivalTime = 0f;
    private float coinTimer = 0f;
    private int currentPrismites = 0;
    public bool isGameOver = false;

    void Start()
    {
        currentPrismites = 0;
        counterData.ResetCounter();
        gameOverPanel.SetActive(false);

        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    void Update()
    {
        if (isGameOver) return;

        survivalTime += Time.deltaTime;
        timeText.text = $"Time: {Mathf.FloorToInt(survivalTime)}s";

        coinTimer += Time.deltaTime;
        if (coinTimer >= 5f)
        {
            currentPrismites += 3;
            coinTimer = 0f;
        }

        pickupText.text = $"Scrap: {counterData.pickupsCollected}";
        coinText.text = $"Prismites: {currentPrismites}";
    }

    public void ShowGameOverPanel()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (resourceManager != null)
        {
            resourceManager.AddPrismites(currentPrismites);
            resourceManager.AddScrap(counterData.pickupsCollected);
        }

        finalCoinsText.text = $"Prismites collected: {currentPrismites}";
        finalPickupsText.text = $"Scrap collected: {counterData.pickupsCollected}";
        finalTimeText.text = $"Time survived: {Mathf.FloorToInt(survivalTime)}s";

        gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(gameScene));
    }
}