using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public FriendshipController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // GET api/values
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationModel>>> Get(string userId)
        {
            var client = _clientFactory.CreateClient("friendsApi");
            var result =
                await client.GetAsync(
                    $"http://friendsapi.eventdrivenmicroservicearchitecture/api/friendship/{userId}");

            if (result.IsSuccessStatusCode == false)
                return Ok(Array.Empty<FriendModel>());

            var notifications = await result.Content.ReadAsAsync<IEnumerable<FriendModel>>();
            if (notifications.Any() == false)
                return Ok(Array.Empty<FriendModel>());


            return Ok(notifications);
        }

        public class FriendModel
        {
            public string id { get; set; }
            public string DisplayName { get; set; }
        }
    }
}