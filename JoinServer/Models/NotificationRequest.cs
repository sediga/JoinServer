using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoinServer.Models
{
    public class NotificationRequest
    {
        public string DeviceId { get; set; }

        public NotificationType RequestNotificationType { get; set; }

        public string ActivityId{ get; set; }
    }
}