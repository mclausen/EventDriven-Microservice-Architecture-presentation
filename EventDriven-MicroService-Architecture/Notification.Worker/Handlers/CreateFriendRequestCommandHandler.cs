using System;
using System.Net.Http;
using System.Threading.Tasks;
using Notification.Domain;
using Notifications.Messages.External;
using Rebus.Handlers;
using SharedKernel;

namespace Notification.Worker.Handlers
{
    public class CreateFriendRequestCommandHandler : IHandleMessages<CreateFriendRequestCommand>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateFriendRequestCommandHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task Handle(CreateFriendRequestCommand message)
        {
            var profile = await GetProfile(message.FromUserId);

            // Fetch displayName
            var fromUserDisplayName = $"{profile.FirstName} {profile.LastName}";

            var notification = new FriendShipRequestNotification()
            {
                UserId = message.ToUserId,
                FromUserId = message.FromUserId,
                FromUserDisplayName = fromUserDisplayName,
                HasBeenConfirmed = false
            };

            await DocumentDbRepository<FriendShipRequestNotification>
                .CreateItemAsync(notification, notification.UserId, "notifications");
        }

        private async Task<ProfileModel> GetProfile(string userId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var result =
                await httpClient.GetAsync($"http://profileapi.eventdrivenmicroservicearchitecture/api/profile/{userId}");
            var profile = await result.Content.ReadAsAsync<ProfileModel>();

            return profile;
        }

       
    }
}
