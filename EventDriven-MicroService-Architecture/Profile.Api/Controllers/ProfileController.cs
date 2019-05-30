using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Profile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Domain.Profile>> Get(string id)
        {
            var item = await DocumentDbRepository<Domain.Profile>.GetItemAsync(
                id, id, "Profiles");
            
            if (item == null)
                return NotFound();

            return Ok(item);
        }
    }
}
