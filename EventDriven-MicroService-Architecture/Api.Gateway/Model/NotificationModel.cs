namespace Api.Gateway.Controllers
{
    public class NotificationModel
    {
        public string id { get; set; }
        public string UserId { get; set; }
        public string FromUserId { get; set; }
        public string FromUserDisplayName { get; set; }
        public bool HasBeenConfirmed { get; set; }
    }
}