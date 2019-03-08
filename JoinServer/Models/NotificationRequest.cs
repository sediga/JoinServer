using JoinServer.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoinServer.Models
{
    public class NotificationRequest
    {
        public string FromDeviceId { get; set; }

        public string ToDeviceId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType RequestNotificationType { get; set; }

        public string ActivityId{ get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RequestStatus NotificationRequestStatus { get; set; }

    }
}