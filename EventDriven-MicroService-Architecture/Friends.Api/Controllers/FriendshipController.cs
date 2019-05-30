using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Friends.Domain;
using Friends.Infrastructure.Storage;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Friends.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {

        // GET api/values/5
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserVertex>>> Get(string userId)
        {
            ResultSet<dynamic> result;
            
            var gremlinServer = GremlinServerFactory.Create();
            using (var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(),
                GremlinClient.GraphSON2MimeType))
            {
                var query = $"g.V('{userId}').out('friendsWith')";
                result = await gremlinClient.SubmitAsync<dynamic>(query);
            }

            if (result == null || result.Any() == false)
                return Ok(Array.Empty<UserVertex>());

            var list = new List<UserVertex>();
            var json = JsonConvert.SerializeObject(result);
            var jArray = JArray.Parse(json);

            foreach (var item in jArray)
            {
                var id = item.Value<string>("id");

                var propertiesToken = item.Value<JToken>("properties");
                var displayName = propertiesToken.Value<JToken>("DisplayName")[0].Value<string>("value");

                list.Add(new UserVertex()
                {
                    id = id,
                    DisplayName = displayName
                });
            }

            int t = 3;
            return Ok(list);
        }
    }
}
