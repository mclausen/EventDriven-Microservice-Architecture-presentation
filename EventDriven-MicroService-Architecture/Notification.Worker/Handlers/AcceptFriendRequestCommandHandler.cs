using System;
using System.Threading.Tasks;
using Notification.Domain;
using Notifications.Messages.External;
using Rebus.Bus;
using Rebus.Handlers;
using SharedKernel;

namespace Notification.Worker.Handlers
{
    public class AcceptFriendRequestCommandHandler : IHandleMessages<AcceptFriendRequestCommand>
    {
        private readonly IBus _bus;

        public AcceptFriendRequestCommandHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(AcceptFriendRequestCommand message)
        {
            var notification = await DocumentDbRepository<FriendShipRequestNotification>
                .GetItemAsync(message.NotificationId, message.UserId, "notifications");

            notification.HasBeenConfirmed = true;

            await DocumentDbRepository<FriendShipRequestNotification>
                .UpdateItemAsync(message.NotificationId, "notifications", message.UserId, notification);

            await _bus.Publish(new FriendShipAcceptedEvent()
            {
                FromUserId = notification.FromUserId,
                ToUserId = notification.UserId
            });
        }
    }
}
