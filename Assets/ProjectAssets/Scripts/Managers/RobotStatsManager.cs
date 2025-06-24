using UnityEngine;
using System.Collections;

public class RobotStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RobotNeeds needsConfig;
    [SerializeField] private UI_NeedsBars needsBars;
    [SerializeField] private ResourceManager resourceManager;

    private int ArmorBarIndex = 0;
    private int PowerBarIndex = 1;
    private int FunBarIndex = 2;

    public RobotNeeds NeedsConfig
    {
        get
        {
            return needsConfig;
        }
    }

    void Start()
    {
        needsConfig.Armor = Mathf.Clamp01(needsConfig.Armor);
        needsConfig.Power = Mathf.Clamp01(needsConfig.Power);
        needsConfig.Fun = Mathf.Clamp01(needsConfig.Fun);

        needsBars.SetBarValue(ArmorBarIndex, needsConfig.Armor);
        needsBars.SetBarValue(PowerBarIndex, needsConfig.Power);
        needsBars.SetBarValue(FunBarIndex, needsConfig.Fun);

        StartCoroutine(DecayNeeds());
    }

    private IEnumerator DecayNeeds()
    {
        yield return new WaitForSeconds(needsConfig.decayInterval);

        needsConfig.Armor = Mathf.Clamp01(needsConfig.Armor - needsConfig.globalDecayRate);
        needsConfig.Power = Mathf.Clamp01(needsConfig.Power - needsConfig.globalDecayRate);
        needsConfig.Fun = Mathf.Clamp01(needsConfig.Fun - needsConfig.globalDecayRate);

        needsBars.SetBarValueAnimated(ArmorBarIndex, needsConfig.Armor);
        needsBars.SetBarValueAnimated(PowerBarIndex, needsConfig.Power);
        needsBars.SetBarValueAnimated(FunBarIndex, needsConfig.Fun);

        needsConfig.SendNotificationPower();
        needsConfig.SendNotificationFun();
        needsConfig.SendNotificationArmor();

        StartCoroutine(DecayNeeds());
    }

    public void RepairArmor(float amount, int scrapUsed)
    {
        if (!resourceManager.CanAffordScrap(scrapUsed)) return;

        resourceManager.RemoveScrap(scrapUsed);
        float previousArmor = needsConfig.Armor;

        needsConfig.Armor = Mathf.Clamp01(needsConfig.Armor + amount);

        if (!Mathf.Approximately(needsConfig.Armor, previousArmor))
        {
            needsBars.SetBarValueAnimated(ArmorBarIndex, needsConfig.Armor);
        }

        needsConfig.SendNotificationArmor();
    }

    public bool CanRepair(int scrapAmount)
    {
        return resourceManager.CanAffordScrap(scrapAmount);
    }
}