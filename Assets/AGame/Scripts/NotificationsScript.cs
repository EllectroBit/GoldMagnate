using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotificationSamples;
using System;

public class NotificationsScript : MonoBehaviour
{
    [SerializeField]
    private GameNotificationsManager gameNotificationsManagerF;
    [SerializeField]
    private GameNotificationsManager gameNotificationsManagerU;

    private void Start()
    {
        if(Application.platform == RuntimePlatform.Android)
            InitializeNotifications();
    }

    private void InitializeNotifications()
    {
        GameNotificationChannel channelF = new GameNotificationChannel("forgeringDone", "Forgering Done", "You have new ingots");
        gameNotificationsManagerF.Initialize(channelF);
        GameNotificationChannel channelU = new GameNotificationChannel("upgradingDone", "Upgrading Done", "Forge has new LVL");
        gameNotificationsManagerU.Initialize(channelU);
    }

    public void CreateNotificationF(string title, string body, long time)
    {
        IGameNotification notification = gameNotificationsManagerF.CreateNotification();
        if(notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.SmallIcon = "icon_1";
            notification.DeliveryTime = DateTime.Now.AddSeconds(time);
            gameNotificationsManagerF.ScheduleNotification(notification);
        }
    }

    public void CreateNotificationU(string title, string body, long time)
    {
        IGameNotification notification = gameNotificationsManagerU.CreateNotification();
        if (notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.LargeIcon = "icon_0";
            notification.DeliveryTime = DateTime.Now.AddSeconds(time);
            gameNotificationsManagerU.ScheduleNotification(notification);
        }
    }
}
