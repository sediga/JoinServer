using JoinServer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoinServer.Models
{
    public class ActivityRequest
    {
        public Guid ActivityRequestId { get; set; }
        public string RequestFrom { get; set; }
        public string RequestTo { get; set; }   
        public DateTime RequestDate { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string ActivityId { get; set; }
        public NotificationType RequestType { get; set; }
        public DateTime RequestStatusChangeDate { get; set; }
    }
}