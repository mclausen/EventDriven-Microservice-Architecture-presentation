namespace Notifications.Messages.External
{
    public class CreateFriendRequestCommand
    {
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
    }
}