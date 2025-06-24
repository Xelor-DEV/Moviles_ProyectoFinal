using UnityEngine;
using Unity.Notifications.Android;

[CreateAssetMenu(menuName = "Notifications/Channel Data")]
public class NotificationChannelData : ScriptableObject
{
    public string Id;
    public string Name;
    public Importance Importance;
    [TextArea] public string Description;
}