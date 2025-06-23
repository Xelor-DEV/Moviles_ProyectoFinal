using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class TargetGameUIManager : MonoBehaviour
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


    [Header("Datos")]
    public CoinData coinData;

    private int hitCount = 0;

    public static TargetGameUIManager Instance;

    private void Awake()
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

    private void Start()
    {
        coinData.ResetCoins();
        hitCount = 0;
        UpdateUI();
        gameOverPanel.SetActive(false);
        retryButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(GoToMenu);
    }

    public void AddHit()
    {
        hitCount++;
        coinData.AddCoins(3); 
        UpdateUI();
    }

    void UpdateUI()
    {
        coinText.text = $"Coins: {coinData.coins}";
        hitsText.text = $"Successes: {hitCount}";
    }
    public void ShowGameOverPanel()
    {
        
        finalCoinsText.text = $"Coins collected: {coinData.coins}";
        finalHitsText.text = $"Successes achieved: {hitCount}";
        gameOverPanel.SetActive (true);
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
        SceneManager.LoadScene("Workshop");
    }
}
