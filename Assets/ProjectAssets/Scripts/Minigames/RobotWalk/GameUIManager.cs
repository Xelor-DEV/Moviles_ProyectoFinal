using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("Tiempo")]
    public float totalTime = 100f;
    private float currentTime;

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

    [Header("Monedas")]
    public CoinData coinData;

    private int correctSwipes = 0;
    private float timeSurvived = 0f;

    public static GameUIManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        coinData.ResetCoins();
        currentTime = totalTime;
        UpdateUI();
        gameOverPanel.SetActive(false);

        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        timeSurvived += Time.deltaTime;

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
        coinText.text = coinData.coins.ToString();
    }


    public void HandleCorrectSwipe()
    {
        if(currentTime < 97)
        {
            currentTime += 3;
        }
        if (currentTime < 98)
        {
            currentTime += 2;
        }
        if (currentTime < 99)
        {
            currentTime += 1;
        }
        correctSwipes++;

        if (correctSwipes >= 3)
        {
            int reward = timeSurvived >= 20f ? 5 : 3;
            coinData.AddCoins(reward);
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
        
        Debug.Log("Volver al menú principal (puedes cargar otra escena aquí)");
        // SceneManager.LoadScene("Menu"); 
    }

    void EndGame()
    {
        
        Debug.Log("Tiempo terminado. Juego finalizado.");

        finalCoinsText.text = $"Monedas conseguidas: {coinData.coins}";
        finalTimeText.text = $"Tiempo sobrevivido: {Mathf.FloorToInt(timeSurvived)}s";
        gameOverPanel.SetActive(true);
    }
}