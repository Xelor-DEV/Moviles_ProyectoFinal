using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Serializable]
    public class PrismitesConversionItem
    {
        public TMP_Text costText;
        public TMP_Text rewardText;
        public int prismitesCost;
        public int energyCoresReward;
    }

    [Serializable]
    public class SkinShopItem
    {
        public RobotSkinData skinData;
        public TMP_Text costText;
        public TMP_Text skinNameText;
        public Image iconPrismites;
        public Button button;
        public int prismitesCost;
    }

    [Header("Prismites Conversion")]
    [SerializeField] private PrismitesConversionItem[] conversionItems;

    [Header("Skin Shop")]
    [SerializeField] private SkinShopItem[] skinItems;

    [Header("Dependencies")]
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private ResourceData resourceData;
    [SerializeField] private SkinManager skinManager;
    [SerializeField] private UI_Resources uiResources;
    [SerializeField] private UI_Closet uiCloset;

    private void Start()
    {
        InitializeConversionItems();
        InitializeSkinItems();
    }
    private void OnEnable()
    {
        DatabaseManager.OnLoadData += InitializeConversionItems;
        DatabaseManager.OnLoadData += InitializeSkinItems;
    }
    private void OnDisable()
    {
        DatabaseManager.OnLoadData -= InitializeConversionItems;
        DatabaseManager.OnLoadData -= InitializeSkinItems;
    }

    private void InitializeConversionItems()
    {
        for (int i = 0; i < conversionItems.Length; ++i)
        {
            conversionItems[i].costText.text = "x" + conversionItems[i].prismitesCost;
            conversionItems[i].rewardText.text = "x" + conversionItems[i].energyCoresReward;
        }
    }

    private void InitializeSkinItems()
    {
        for (int i = 0; i < skinItems.Length; ++i)
        {
            UpdateSkinItemUI(i);
            skinItems[i].skinNameText.text = skinItems[i].skinData.SkinName;
        }
    }

    private void UpdateSkinItemUI(int index)
    {
        bool isUnlocked = skinManager.IsSkinUnlocked(skinItems[index].skinData.SkinId);

        if (isUnlocked)
        {
            skinItems[index].costText.text = "";
            skinItems[index].iconPrismites.gameObject.SetActive(false);
            skinItems[index].button.interactable = false;
            
            uiCloset.UnlockSkin(index);
        }
        else
        {
            skinItems[index].costText.text = "x" + skinItems[index].prismitesCost;
            skinItems[index].iconPrismites.gameObject.SetActive(true);
            skinItems[index].button.interactable = true;

            uiCloset.LockSkin(index);
        }
    }

    public void BuyConversion(int index)
    {
        if (index < 0 || index >= conversionItems.Length)
        {
            Debug.LogError("Invalid conversion index: " + index);
            return;
        }

        PrismitesConversionItem item = conversionItems[index];

        if (resourceManager.CanAffordPrismites(item.prismitesCost))
        {
            resourceManager.RemovePrismites(item.prismitesCost);
            resourceManager.AddEnergyCores(item.energyCoresReward);

            DatabaseManager.Instance.SaveAllData();

            uiResources.UpdatePrismitesText(resourceData.Prismites);
            uiResources.UpdateEnergyCoresText(resourceData.EnergyCores);
        }
        else
        {
            Debug.Log("You do not have enough prismites");
        }
    }

    public void BuySkin(int index)
    {
        if (index < 0 || index >= skinItems.Length)
        {
            Debug.LogError("Invalid skin index: " + index);
            return;
        }

        SkinShopItem item = skinItems[index];

        if (skinManager.IsSkinUnlocked(item.skinData.SkinId))
        {
            Debug.Log("You already have this skin unlocked");
            return;
        }

        if (resourceManager.CanAffordPrismites(item.prismitesCost))
        {
            resourceManager.RemovePrismites(item.prismitesCost);
            skinManager.UnlockSkin(item.skinData.SkinId);

            UpdateSkinItemUI(index);

            DatabaseManager.Instance.SaveAllData();
            uiResources.UpdatePrismitesText(resourceData.Prismites);
            Debug.Log("Skin unlocked: " + item.skinData.SkinName);
        }
        else
        {
            Debug.Log("You do not have enough prismites for this skin");
        }
    }
}