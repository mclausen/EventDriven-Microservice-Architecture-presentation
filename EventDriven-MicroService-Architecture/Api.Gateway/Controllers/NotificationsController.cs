using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notifications.Messages.External;
using Rebus.Bus;

namespace Api.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationsController(IBus bus, IHttpClientFactory httpClientFactory)
        {
            _bus = bus;
            _httpClientFactory = httpClientFactory;
        }

        // GET api/values
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationModel>>> Get(string userId)
        {
            var client = _httpClientFactory.CreateClient("notificatinsApi");
            var result =
                await client.GetAsync(
                    $"http://notificationapi.eventdrivenmicroservicearchitecture/api/notifications/{userId}");

            if (result.IsSuccessStatusCode == false)
                return Ok(Array.Empty<NotificationModel>());

            var notifications = await result.Content.ReadAsAsync<IEnumerable<NotificationModel>>();
            if (notifications.Any() == false)
                return Ok(Array.Empty<NotificationModel>());


            return Ok(notifications);
        }

        // POST api/values
        [HttpPost("friendrequests")]
        public async Task<IActionResult> Post([FromBody] SendFriendRequest value)
        {
            var cmd = new CreateFriendRequestCommand()
            {
                FromUserId = value.FromId,
                ToUserId = value.ToId
            };

            await _bus.Send(cmd);
            return Accepted();
        }

        [HttpPost("requests/{userId}/accept/{notificationId}")]
        public async Task<IActionResult> Post(string userId, string notificationId)
        {
            var cmd = new AcceptFriendRequestCommand()
            {
                UserId = userId,
                NotificationId = notificationId
            };

            await _bus.Send(cmd);
            return Accepted();
        }
    }
}