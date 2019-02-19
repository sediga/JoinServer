﻿namespace JoinServer.Models
{
    public class Device
    {
        public string DeviceID { get; set; }
        public string EmailID { get; set; }
        public string SoftwareVersion { get; set; }
        public string NotificationToken { get; set; }
    }

    public class Message
    {
        public string[] registration_ids { get; set; }
        public Notification notification { get; set; }
        public object data { get; set; }
    }

    public class Notification
    {
        public string title { get; set; }
        public string text { get; set; }
    }

    public enum NotificationType
    {
        None,
        AmIn
    }
}