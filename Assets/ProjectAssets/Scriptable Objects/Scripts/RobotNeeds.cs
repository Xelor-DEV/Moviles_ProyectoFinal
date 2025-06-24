using UnityEngine;

[CreateAssetMenu(fileName = "RobotNeeds", menuName = "Robot/Needs")]
public class RobotNeeds : ScriptableObject
{
    [Header("Values")]
    [Range(0, 1)]
    [SerializeField] private float armor = 0.5f;
    [Range(0, 1)] 
    [SerializeField] private float power = 0.5f;
    [Range(0, 1)] 
    [SerializeField] private float fun = 0.5f;

    [Header("Decay Settings")]
    [Tooltip("Percentage decay per interval (0.01 = 1%)")]
    public float globalDecayRate = 0.01f;
    public float decayInterval = 15f;

    [Header("Armor Settings")]
    public float armorRepairPerScrap = 0.05f;
    public float minArmorForDamage = 0.2f;

    [Header("Power Station Settings")]
    public float rechargePowerPerCore = 0.25f;
    public float rechargeTimePerCore = 5f;

    [Header("Notification Threshold")]
    [SerializeField] private float critical;
    [SerializeField] private float max;

    public float Armor
    {
        get
        {
            return armor;
        }
        set
        {
            if(armor > max)
            {
                NotificationSystem.Instance.SendNotification(NotificationInvoker.Instance.ArmorHigh);
            }
            else if(armor < critical)
            {
                NotificationSystem.Instance.SendNotification(NotificationInvoker.Instance.ArmorLow);
            }

            armor = value; 
        }
    }

    public float Power
    {
        get
        {
            return power;
        }
        set
        {
            if (power > max)
            {
                NotificationSystem.Instance.SendNotification(NotificationInvoker.Instance.PowerHigh);
            }
            else if (power < critical)
            {
                NotificationSystem.Instance.SendNotification(NotificationInvoker.Instance.PowerLow);
            }

            power = value;
        }
    }

    public float Fun
    {
        get
        {
            return fun;
        }
        set
        {
            if (fun > max)
            {
                NotificationSystem.Instance.SendNotification(NotificationInvoker.Instance.FunHigh);
            }
            else if (fun < critical)
            {
                NotificationSystem.Instance.SendNotification(NotificationInvoker.Instance.FunLow);
            }

            fun = value;
        }
    }
}