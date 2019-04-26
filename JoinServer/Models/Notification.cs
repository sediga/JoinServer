using JoinServer.Utilities;
using System;


namespace JoinServer.Models
{
    public class NotificationDetails
    {
        public string NotificationId { get; set; }
        public string ActivityId { get; set; }
        public string DeviceId { get; set; }
        public string NotificationText { get; set; }
        public MessageStatuses MessageStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public object MessageObject { get; set; }
        public bool Dismissed { get; set; }
        public string RequestId { get; set; }
    }
}