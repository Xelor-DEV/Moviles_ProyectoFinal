using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : NonPersistentSingleton<GameUIManager>
{
    [Header("Tiempo")]
    public float totalTime = 100f;
    private float currentTime;
    public float timeDecreaseSpeed = 1.5f;

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
    private int currentPrismites;
    private int correctSwipes = 0;
    private int correctSwipesSpeed = 0;
    private float timeSurvived = 0f;

    void Start()
    {
        Time.timeScale = 1f;
        currentPrismites = 0;
        currentTime = totalTime;
        UpdateUI();
        gameOverPanel.SetActive(false);

        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    void Update()
    {
        currentTime -= Time.deltaTime * timeDecreaseSpeed;
        timeSurvived += Time.deltaTime;

        if (correctSwipesSpeed > 20)
        {
            timeDecreaseSpeed = 3.5f;
        }
        else if (correctSwipesSpeed > 50)
        {
            timeDecreaseSpeed = 5;
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
        coinText.text = $"Prismites: {currentPrismites}";
    }

    public void HandleCorrectSwipe()
    {
        if (currentTime < 99)
        {
            currentTime += 1;
        }
        correctSwipes++;
        correctSwipesSpeed++;
        if (correctSwipes >= 3)
        {
            int reward = timeSurvived >= 20f ? 5 : 3;
            currentPrismites += reward;
            correctSwipes = 0;
        }
    }

    public void HandleWrongSwipe()
    {
        currentTime -= 15f;
        if (currentTime < 0)
        {
            currentTime = 0;
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void GoToMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(GlobalSceneManager.Instance.LoadSceneAsync(gameScene));
    }

    void EndGame()
    {
        if (resourceManager != null)
        {
            resourceManager.AddPrismites(currentPrismites);
        }

        finalCoinsText.text = $"Prismites collected: {currentPrismites}";
        finalTimeText.text = $"Time survived: {Mathf.FloorToInt(timeSurvived)}s";
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}