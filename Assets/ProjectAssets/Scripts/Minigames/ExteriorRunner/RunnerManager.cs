using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class RunnerManager : MonoBehaviour
{
    [Header("Referencias")]
    public ScrashCounter counterData;
    public CoinData coinData;

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

    public static RunnerManager Instance;

    private float survivalTime = 0f;
    private float coinTimer = 0f;

    private bool isGameOver = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        coinData.ResetCoins();
        gameOverPanel.SetActive(false);

        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    void Update()
    {
        if (isGameOver) return;

        survivalTime += Time.deltaTime;
        timeText.text = $"Tiempo: {Mathf.FloorToInt(survivalTime)}s";

        coinTimer += Time.deltaTime;
        if (coinTimer >= 5f)
        {
            coinData.AddCoins(3);
            coinTimer = 0f;
        }

        pickupText.text = $"Recolectados: {counterData.pickupsCollected}";
        coinText.text = $"Monedas: {coinData.coins}";
    }

    public void ShowGameOverPanel()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        finalCoinsText.text = $"Monedas conseguidas: {coinData.coins}";
        finalPickupsText.text = $"Recolectados: {counterData.pickupsCollected}";
        finalTimeText.text = $"Tiempo sobrevivido: {Mathf.FloorToInt(survivalTime)}s";

        gameOverPanel.SetActive(true);
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMenu()
    {
        
        Debug.Log("Escena Menu");
    }
}
