using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Profile.Message.External;
using Rebus.Bus;

namespace Api.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IBus bus, IHttpClientFactory httpClientFactory)
        {
            _bus = bus;
            _httpClientFactory = httpClientFactory;
        }

        // GET api/values
        [HttpGet]
        [HttpGet("{userId}")]
        public async  Task<ActionResult<ProfileModel>> Get(string userId)
        {
            var client = _httpClientFactory.CreateClient();
            var result =
                await client.GetAsync($"http://profileapi.eventdrivenmicroservicearchitecture/api/profile/{userId}");

            if (result.IsSuccessStatusCode == false)
                return NotFound();

            var profile = await result.Content.ReadAsAsync<ProfileModel>();

            return Ok(profile);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProfileRequest value)
        {
            var userId = Guid.NewGuid().ToString();
            var cmd = new CreateProfileCommand
            {
                UserId = userId,
                FirstName = value.FirstName,
                LastName = value.LastName
            };

            await _bus.Send(cmd);
            return Accepted(cmd);
        }

        // POST api/values
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] ProfileModel value)
        {
            var cmd = new UpdateProfileCommand
            {
                Id = userId,
                FirstName = value.FirstName,
                LastName = value.LastName
            };

            await _bus.Send(cmd);
            return Accepted();
        }

        
    }
}
