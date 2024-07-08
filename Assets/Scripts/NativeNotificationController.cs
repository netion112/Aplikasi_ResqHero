using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NativeNotificationController : MonoBehaviour
{
    [SerializeField] private AndroidNotificationController _androidNotificationController;
    public Toggle switchToggle;
    
    void Start()
    {
        if(!switchToggle.isOn)
        {
            _androidNotificationController.RequestAuthorization();
            _androidNotificationController.RegisterNotificationChannel();
            _androidNotificationController.SendNotification("ResqHero", "Ayo bermain lagi untuk meningkatkan pengalaman bermain", 3);
        }
    }
}
