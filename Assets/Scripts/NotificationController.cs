using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
    [SerializeField] private Android_Notification androidNotifications;

    public static bool notificationsEnabled = true; // Static untuk akses dari skrip lain

    private void Start()
    {
        androidNotifications.RequestAuthorization();
        androidNotifications.RegisterNotificationChannel();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && notificationsEnabled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            androidNotifications.SendNotification("ResqHero", "Tanggap darurat: Harap tetap tenang dan ikuti petunjuk keselamatan yang tersedia di aplikasi ResQHero.", 2);
        }
    }

    public void EnableNotifications()
    {
        notificationsEnabled = true;
        Debug.Log("Notifications Enabled");
    }

    public void DisableNotifications()
    {
        notificationsEnabled = false;
        Debug.Log("Notifications Disabled");
    }
}
