using UnityEngine;
using System.Collections;

public class RobotStatsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RobotNeeds needsConfig;
    [SerializeField] private UI_NeedsBars needsBars;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private NotificationInvoker notificationInvoker;
    [SerializeField] private NotificationSystem notificationSystem;

    private int ArmorBarIndex = 0;
    private int PowerBarIndex = 1;
    private int FunBarIndex = 2;

    private bool armorCriticalNotified = false;
    private bool armorMaxNotified = false;
    private bool powerCriticalNotified = false;
    private bool powerMaxNotified = false;
    private bool funCriticalNotified = false;
    private bool funMaxNotified = false;

    public RobotNeeds NeedsConfig
    {
        get
        {
            return needsConfig;
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        DatabaseManager.OnLoadData += Initialize;
    }

    private void OnDisable()
    {
        DatabaseManager.OnLoadData -= Initialize;
    }
    private void Initialize()
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

        DatabaseManager.Instance.SaveAllData();

        SendNotificationArmor();
        SendNotificationPower();
        SendNotificationFun();

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

        SendNotificationArmor();
    }

    public bool CanRepair(int scrapAmount)
    {
        return resourceManager.CanAffordScrap(scrapAmount);
    }

    public void SendNotificationArmor()
    {
        if (needsConfig.Armor >= needsConfig.Critical && needsConfig.Armor <= needsConfig.Max)
        {
            armorCriticalNotified = false;
            armorMaxNotified = false;
            return;
        }

        if (needsConfig.Armor > needsConfig.Max && !armorMaxNotified)
        {
            notificationSystem.SendNotification(notificationInvoker.ArmorHigh);
            armorMaxNotified = true;
            armorCriticalNotified = false;
        }
        else if (needsConfig.Armor < needsConfig.Critical && !armorCriticalNotified)
        {
            notificationSystem.SendNotification(notificationInvoker.ArmorLow);
            armorCriticalNotified = true;
            armorMaxNotified = false;
        }
    }

    public void SendNotificationPower()
    {
        if (needsConfig.Power >= needsConfig.Critical && needsConfig.Power <= needsConfig.Max)
        {
            powerCriticalNotified = false;
            powerMaxNotified = false;
            return;
        }

        if (needsConfig.Power > needsConfig.Max && !powerMaxNotified)
        {
            notificationSystem.SendNotification(notificationInvoker.PowerHigh);
            powerMaxNotified = true;
            powerCriticalNotified = false;
        }
        else if (needsConfig.Power < needsConfig.Critical && !powerCriticalNotified)
        {
            notificationSystem.SendNotification(notificationInvoker.PowerLow);
            powerCriticalNotified = true;
            powerMaxNotified = false;
        }
    }

    public void SendNotificationFun()
    {
        if (needsConfig.Fun >= needsConfig.Critical && needsConfig.Fun <= needsConfig.Max)
        {
            funCriticalNotified = false;
            funMaxNotified = false;
            return;
        }

        if (needsConfig.Fun > needsConfig.Max && !funMaxNotified)
        {
            notificationSystem.SendNotification(notificationInvoker.FunHigh);
            funMaxNotified = true;
            funCriticalNotified = false;
        }
        else if (needsConfig.Fun < needsConfig.Critical && !funCriticalNotified)
        {
            notificationSystem.SendNotification(notificationInvoker.FunLow);
            funCriticalNotified = true;
            funMaxNotified = false;
        }
    }
}