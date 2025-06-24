using UnityEngine;
using TMPro;

public class UI_Resources : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private ResourceData resourceData;

    [Header("UI Text Elements")]
    [SerializeField] private TMP_Text scrapText;
    [SerializeField] private TMP_Text prismitesText;
    [SerializeField] private TMP_Text energyCoresText;

    private void OnEnable()
    {
        DatabaseManager.Instance.OnLoadData += InitializeUI;
    }

    private void OnDisable()
    {
        DatabaseManager.Instance.OnLoadData -= InitializeUI;
    }

    public void InitializeUI()
    {
        if (resourceData == null)
        {
            Debug.LogError("ResourceData is not assigned in UI_Resources.");
            return;
        }
        UpdateScrapText(resourceData.Scrap);
        UpdatePrismitesText(resourceData.Prismites);
        UpdateEnergyCoresText(resourceData.EnergyCores);
    }

    public void UpdateScrapText(int amount)
    {
        if (scrapText != null)
        {
            scrapText.text = "x" + amount;
        }
    }

    public void UpdatePrismitesText(int amount)
    {
        if (prismitesText != null)
        {
            prismitesText.text = "x" + amount;
        }
    }

    public void UpdateEnergyCoresText(int amount)
    {
        if (energyCoresText != null)
        {
            energyCoresText.text = "x" + amount;
        }
    }
}