using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notification.Domain;
using SharedKernel;

namespace Notification.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        // GET api/values/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<FriendShipRequestNotification>>> Get(string userId)
        {
            var items = await DocumentDbRepository<FriendShipRequestNotification>.GetItemsAsync(
                d => d.UserId == userId && d.HasBeenConfirmed == false, "notifications");

            if (items == null)
                return Ok(Array.Empty<FriendShipRequestNotification>());

            return Ok(items);
        }
    }
}
