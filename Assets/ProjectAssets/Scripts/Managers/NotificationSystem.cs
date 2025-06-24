using UnityEngine;
using System;

using Unity.Notifications.Android;
using UnityEngine.Android;


public class NotificationSystem : NonPersistentSingleton<NotificationSystem>
{
    [Header("Notification Settings")]
    [SerializeField] private NotificationChannelData[] notificationChannels;
    [SerializeField] private NotificationConfig[] notifications;

    public NotificationConfig[] Notifications
    {
        get
        {
            return notifications;
        }
    }

    public NotificationChannelData[] NotificationChannels
    {
        get
        {
            return notificationChannels;
        }
    }

    void Start()
    {
        RequestAuthorization();
        RegisterAllNotificationChannels();
    }

    private void RequestAuthorization()
    {
        if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS") == false)
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    private void RegisterAllNotificationChannels()
    {
        for (int i = 0; i < notificationChannels.Length; ++i)
        {
            RegisterNotificationChannel(notificationChannels[i]);
        }
    }

    public void RegisterNotificationChannel(NotificationChannelData channelData)
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel
        {
            Id = channelData.Id,
            Name = channelData.Name,
            Importance = channelData.Importance,
            Description = channelData.Description
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void RegisterNotificationChannel(int index)
    {
        RegisterNotificationChannel(notificationChannels[index]);
    }

    public void SendNotification(int index)
    {
        SendNotification(notifications[index]);
    }

    public void SendNotification(NotificationConfig config)
    {
        var notification = new AndroidNotification
        {
            Title = config.Title,
            Text = config.Text,
            FireTime = DateTime.Now,
            SmallIcon = config.SmallIcon,
            LargeIcon = config.LargeIcon
        };

        AndroidNotificationCenter.SendNotification(notification, config.Channel.Id);

        Debug.Log("noti");
    }

    public void SendNotification(NotificationConfig config, DateTime fireTime)
    {
        var notification = new AndroidNotification
        {
            Title = config.Title,
            Text = config.Text,
            FireTime = fireTime,
            SmallIcon = config.SmallIcon,
            LargeIcon = config.LargeIcon
        };

        AndroidNotificationCenter.SendNotification(notification, config.Channel.Id);
    }
}