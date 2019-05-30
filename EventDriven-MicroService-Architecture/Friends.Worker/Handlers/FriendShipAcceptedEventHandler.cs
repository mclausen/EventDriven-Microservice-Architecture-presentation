using System.Threading.Tasks;
using Friends.Infrastructure.Storage;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Notifications.Messages.External;
using Rebus.Handlers;

namespace Friends.Worker.Handlers
{
    public class FriendShipAcceptedEventHandler : IHandleMessages<FriendShipAcceptedEvent>
    {
        public async Task Handle(FriendShipAcceptedEvent message)
        {
            var gremlinServer = GremlinServerFactory.Create();
            using (var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader(), new GraphSON2Writer(),
                GremlinClient.GraphSON2MimeType))
            {
                // Bidirectional friendship
                // From user -> To user
                await CreateEdge(gremlinClient, message.FromUserId, message.ToUserId);

                // To User -> From User
                await CreateEdge(gremlinClient, message.ToUserId, message.FromUserId);
            }
        }

        private async Task CreateEdge(GremlinClient gremlinClient, string fromUserId, string toUserId)
        {
            var query = $"g.V('{fromUserId}').addE('friendsWith').to(g.V('{toUserId}'))";
            await gremlinClient.SubmitAsync<dynamic>(query);
        }
    }
}