using UnityEngine;

[CreateAssetMenu(menuName = "Notifications/Notification Config")]
public class NotificationConfig : ScriptableObject
{
    public string Title;
    [TextArea] public string Text;
    public string SmallIcon;
    public string LargeIcon;
    public NotificationChannelData Channel;
}

