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
}