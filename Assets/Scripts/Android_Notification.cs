using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;
using UnityEngine.Android;

public class Android_Notification : MonoBehaviour
{
    //Request authorization to send notifications
    public void RequestAuthorization()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    // Register a notification channel
    public void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Full Lives"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    // Set up notification template
    public void SendNotification(string title, string text, int fireTimeInHours)
    {
        var notification = new Unity.Notifications.Android.AndroidNotification
        {
            Title = title,
            Text = text,
            FireTime = DateTime.Now.AddHours(fireTimeInHours),
            SmallIcon = "icon_0",
            LargeIcon = "icon_0"
        };

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }

    
}
