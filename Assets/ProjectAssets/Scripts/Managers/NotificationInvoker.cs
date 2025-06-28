using UnityEngine;

public class NotificationInvoker : MonoBehaviour
{
    [Header("Notifications Data References")]
    [SerializeField] private NotificationConfig funHigh;
    [SerializeField] private NotificationConfig funLow;
    [SerializeField] private NotificationConfig armorHigh;
    [SerializeField] private NotificationConfig armorLow;
    [SerializeField] private NotificationConfig powerHigh;
    [SerializeField] private NotificationConfig powerLow;

    public NotificationConfig FunHigh
    {
        get
        {
            return funHigh;
        }
    }
    public NotificationConfig FunLow
    {
        get
        {
            return funLow;
        }
    }
    public NotificationConfig ArmorHigh
    {
        get
        {
            return armorHigh;
        }
    }
    public NotificationConfig ArmorLow
    {
        get
        {
            return armorLow;
        }
    }
    public NotificationConfig PowerHigh
    {
        get
        {
            return powerHigh;
        }
    }
    public NotificationConfig PowerLow
    {
        get
        {
            return powerLow;
        }
    }
}