using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RobotStats : MonoBehaviour
{
    public RobotNeeds needsConfig;
    public Image armorBar;
    public TMP_Text scrapText;

    private float currentArmor;
    private int scrapCount = 10;
    private float armorVelocity;

    void Start()
    {
        currentArmor = needsConfig.armor;
        UpdateUI();
    }

    void Update()
    {
        DecayArmor();
        UpdateUI();
    }

    void DecayArmor()
    {
        if (currentArmor > needsConfig.minArmorForDamage)
        {
            currentArmor -= needsConfig.armorDecayRate * Time.deltaTime;
            currentArmor = Mathf.Clamp01(currentArmor);
        }
    }

    void UpdateUI()
    {
        // Animación suave de la barra
        armorBar.fillAmount = Mathf.SmoothDamp(armorBar.fillAmount, currentArmor, ref armorVelocity, 0.2f);

        // Cambio de color basado en el nivel de armadura
        armorBar.color = Color.Lerp(Color.red, Color.green, currentArmor);

        scrapText.text = $"Scrap: {scrapCount}";
    }

    public bool CanRepair(int scrapAmount)
    {
        return scrapCount >= scrapAmount;
    }

    public void RepairArmor(float amount, int scrapUsed)
    {
        currentArmor += amount;
        currentArmor = Mathf.Clamp01(currentArmor);
        scrapCount -= scrapUsed;
    }

    public void AddScrap(int amount)
    {
        scrapCount += amount;
    }
}