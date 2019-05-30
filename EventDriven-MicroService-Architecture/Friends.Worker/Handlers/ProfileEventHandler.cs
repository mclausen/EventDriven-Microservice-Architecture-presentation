using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Friends.Domain;
using Friends.Infrastructure.Storage;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Profile.Message.External;
using Rebus.Handlers;

namespace Friends.Worker.Handlers
{
    public class ProfileEventHandler :
        IHandleMessages<ProfileUpdatedEvent>,
        IHandleMessages<ProfileCreatedEvent>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileEventHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task Handle(ProfileUpdatedEvent message)
        {
            await AddOrUpdate(message.Id);
        }

        public async Task Handle(ProfileCreatedEvent message)
        {
            await AddOrUpdate(message.Id);
        }

        private async Task AddOrUpdate(string userId)
        {
            var profile = await GetProfile(userId);

            var gremlinServer = GremlinServerFactory.Create();
            using (var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(),
                GremlinClient.GraphSON2MimeType))
            {
                var userNode = await GetUserNode(gremlinClient, userId);
                if (userNode == null)
                {
                    await CreateNode(gremlinClient, profile);
                }
                else
                {
                    await UpdateNode(gremlinClient, profile);
                }
            }
        }

        private async Task<ProfileModel> GetProfile(string userId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var result =
                await httpClient.GetAsync($"http://profileapi.eventdrivenmicroservicearchitecture/api/profile/{userId}");
            var profile = await result.Content.ReadAsAsync<ProfileModel>();

            return profile;
        }

        private async Task<UserVertex> GetUserNode(GremlinClient client, string userId)
        {
            var result = await client.SubmitWithSingleResultAsync<dynamic>($"g.V('{userId}')", new Dictionary<string, object>());

            if (result == null)
                return null;

            var json = JsonConvert.SerializeObject(result);
            var token = JObject.Parse(json);
            var id = token.Value<string>("id");
            var propertiesToken = token.Value<JToken>("properties");
            var displayName = propertiesToken.Value<JToken>("DisplayName")[0].Value<string>("value");
            
            return new UserVertex()
            {
                DisplayName = displayName,
                id = id
            };
        }

        private async Task CreateNode(GremlinClient client, ProfileModel profile)
        {
            var displayName = $"{profile.FirstName} {profile.LastName}";
            await client.SubmitAsync<dynamic>($"g.addV('{profile.id}').property('DisplayName', '{displayName}').property('id', '{profile.id}')", new Dictionary<string, object>());
        }

        private async Task UpdateNode(GremlinClient client, ProfileModel profile)
        {
            var displayName = $"{profile.FirstName} {profile.LastName}";
            var server = GremlinServerFactory.Create();

            await client.SubmitAsync<dynamic>($"g.V('{profile.id}').property('DisplayName', '{displayName}')", new Dictionary<string, object>());
        }
    }
}
