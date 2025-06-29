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
        if (needsConfig.Armor > needsConfig.Max)
        {
            notificationSystem.SendNotification(notificationInvoker.ArmorHigh);
            Debug.Log("1");
        }
        else if (needsConfig.Armor < needsConfig.Critical)
        {
            notificationSystem.SendNotification(notificationInvoker.ArmorLow);
            Debug.Log("2");
        }
    }

    public void SendNotificationPower()
    {
        if (needsConfig.Power > needsConfig.Max)
        {
            notificationSystem.SendNotification(notificationInvoker.PowerHigh);
            Debug.Log("3");
        }
        else if (needsConfig.Power < needsConfig.Critical)
        {
            notificationSystem.SendNotification(notificationInvoker.PowerLow);
            Debug.Log("4");
        }
    }

    public void SendNotificationFun()
    {
        if (needsConfig.Fun > needsConfig.Max)
        {
            notificationSystem.SendNotification(notificationInvoker.FunHigh);
            Debug.Log("5");
        }
        else if (needsConfig.Fun < needsConfig.Critical)
        {
            notificationSystem.SendNotification(notificationInvoker.FunLow);
            Debug.Log("6");
        }
    }
}