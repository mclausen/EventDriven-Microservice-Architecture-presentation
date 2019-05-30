using System;

namespace Notifications.Messages.External
{
    public class FriendShipAcceptedEvent
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
    }
}
