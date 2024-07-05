using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeNotificationsController : MonoBehaviour
{
    [SerializeField]
    private AndroidNotificationsController androidNotificationsController;

    private void Start()
    {
        androidNotificationsController.RequestAuthorization();
        androidNotificationsController.RegisterNotificationChannel();
        androidNotificationsController.SendNotification(title: "ResqHero!", text: "This test Notification app", fireTimeInSecond: 3);
    }
}
