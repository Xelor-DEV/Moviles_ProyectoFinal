using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class UI_PowerStation : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image chargingIcon;

    [Header("Sprites")]
    [SerializeField] private Sprite connectedSprite;
    [SerializeField] private Sprite disconnectedSprite;

    [Header("Events")]
    public UnityEvent OnPowerUpdate;

    [Header("Dependencies")]
    [SerializeField] private RobotNeeds robotNeeds;
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private UI_NeedsBars needsBars;
    [SerializeField] private UI_Resources uiResources;
    [SerializeField] private ResourceData resourceData;

    [Header("Settings")]
    [SerializeField] private int PowerBarIndex = 1;

    private bool isCharging = false;
    private Coroutine chargingRoutine;

    void Start()
    {
        SetDisconnectedState();
    }

    public void ToggleCharging()
    {
        if (isCharging)
        {
            SetDisconnectedState();
        }
        else
        {
            if (CanStartCharging())
            {
                SetConnectedState();
                StartCharging();
            }
        }
    }

    private bool CanStartCharging()
    {
        return resourceManager.CanAffordEnergyCores(1) &&
               robotNeeds.Power < 1f;
    }

    private void SetConnectedState()
    {
        isCharging = true;
        chargingIcon.sprite = connectedSprite;

        AudioManager.Instance.PlaySfx(1);

        OnPowerUpdate?.Invoke();
    }

    public void SetDisconnectedState()
    {
        isCharging = false;
        chargingIcon.sprite = disconnectedSprite;

        if (chargingRoutine != null)
        {
            StopCoroutine(chargingRoutine);
            chargingRoutine = null;
        }

        AudioManager.Instance.PlaySfx(2);

        OnPowerUpdate?.Invoke();
    }

    private void StartCharging()
    {
        if (chargingRoutine == null)
        {
            chargingRoutine = StartCoroutine(ChargingProcess());
        }
    }

    private IEnumerator ChargingProcess()
    {
        while (isCharging && CanContinueCharging())
        {
            yield return new WaitForSeconds(robotNeeds.rechargeTimePerCore);

            if (!CanContinueCharging()) break;

            resourceManager.RemoveEnergyCores(1);

            DatabaseManager.Instance.SaveAllData();

            if (uiResources != null)
            {
                uiResources.UpdateEnergyCoresText(resourceData.EnergyCores);
            }

            robotNeeds.Power = Mathf.Clamp01(
                robotNeeds.Power + robotNeeds.rechargePowerPerCore
            );

            needsBars.SetBarValueAnimated(PowerBarIndex, robotNeeds.Power);

            if (robotNeeds.Power >= 1f)
            {
                SetDisconnectedState();
            }

            OnPowerUpdate?.Invoke();
        }

        if (!resourceManager.CanAffordEnergyCores(1))
        {
            SetDisconnectedState();
        }

        chargingRoutine = null;
    }

    private bool CanContinueCharging()
    {
        return resourceManager.CanAffordEnergyCores(1) &&
               robotNeeds.Power < 1f &&
               isCharging;
    }
}