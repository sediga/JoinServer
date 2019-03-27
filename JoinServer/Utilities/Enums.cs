using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JoinServer.Utilities
{
    public enum RequestStatus
    {
        NONE = 0,
        NEW = 1,
        PENDING = 2,
        REQUESTED = 3,
        ACCEPTED = 4,
        REJECTED = 5,
        CANCELLED = 6,
        BLOCKED = 99
    }

    public enum NotificationType
    {
        NONE = 0,
        AMIN = 1
    }

    public enum ActivityTypes
    {
        NONE = 0,
        PUBLIC = 1,
        ONREQUEST = 2
    }

    public enum ActivityStatuses
    {
        OPEN = 0,
        ClOSED = 1,
        REOPENED = 2,
        BLOCKED = 3,
        ONHOLD = 4,
        RESUMED = 5
    }
}