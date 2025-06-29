using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : NonPersistentSingleton<GameUIManager>
{
    [Header("Tiempo")]
    public float totalTime = 100f;
    private float currentTime;
    public float timeDecreaseSpeed = 3.5f;

    [Header("UI")]
    public Slider timeSlider;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI coinText;

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalCoinsText;
    [SerializeField] private TextMeshProUGUI finalTimeText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private string gameScene;
    [SerializeField] private RobotNeeds robotNeeds;
    [SerializeField] private float funAdded = 0.25f;

    private int pendingPrismites = 0;
    private int correctSwipes = 0;
    private int correctSwipesSpeed = 0;
    private float timeSurvived = 0f;
    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1f;
        pendingPrismites = 0;
        currentTime = totalTime;
        UpdateUI();
        gameOverPanel.SetActive(false);
        gameEnded = false;

        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime * timeDecreaseSpeed;
        timeSurvived += Time.deltaTime;

        if (correctSwipesSpeed > 50)
        {
            timeDecreaseSpeed = 5.2f;
        }
        else if (correctSwipesSpeed > 20)
        {
            timeDecreaseSpeed = 4.2f;
        }

        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        timeSlider.value = currentTime / totalTime;
        timeText.text = Mathf.CeilToInt(currentTime).ToString();
        coinText.text = $"Prismites: {pendingPrismites}";
    }

    public void HandleCorrectSwipe()
    {
        if (gameEnded) return;

        currentTime = Mathf.Min(currentTime + 1f, 99f);

        correctSwipes++;
        correctSwipesSpeed++;

        if (correctSwipes >= 5)
        {
            pendingPrismites++;
            correctSwipes = 0;
        }
    }

    public void HandleWrongSwipe()
    {
        if (gameEnded) return;

        currentTime = Mathf.Max(currentTime - 15f, 0f);
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        robotNeeds.Fun = Mathf.Clamp01(robotNeeds.Fun + funAdded);
        StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(gameScene));
    }

    void EndGame()
    {
        gameEnded = true;

        if (pendingPrismites > 0 && resourceManager != null)
        {
            resourceManager.AddPrismites(pendingPrismites);
        }

        finalCoinsText.text = $"Prismites collected: {pendingPrismites}";
        finalTimeText.text = $"Time survived: {Mathf.FloorToInt(timeSurvived)}s";
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}