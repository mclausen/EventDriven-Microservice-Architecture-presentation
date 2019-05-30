namespace Notifications.Messages.External
{
    public class AcceptFriendRequestCommand
    {
        public string UserId { get; set; }
        public string NotificationId { get; set; }
    }
}