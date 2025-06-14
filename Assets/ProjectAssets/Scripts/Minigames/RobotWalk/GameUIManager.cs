using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Tiempo")]
    public float totalTime = 100f;
    private float currentTime;

    [Header("UI")]
    public Slider timeSlider;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI coinText;

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
        currentTime += 3;
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

    void EndGame()
    {
        Time.timeScale = 0;
        Debug.Log("Tiempo terminado. Juego finalizado.");
    }
}