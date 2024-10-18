using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class MobileNotifications : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notifications Channel",
            Importance = Importance.Default,
            Description = "Reminder notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification();
        notification.Title = "Daily Reward!";
        notification.Text = "Your daily reward is ready to collect!";
        notification.FireTime = System.DateTime.Now.AddMinutes(1360);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";


        if(AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");

        }

    }


}
