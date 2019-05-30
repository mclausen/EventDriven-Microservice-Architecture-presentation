using Gremlin.Net.Driver;
using SharedKernel;

namespace Friends.Infrastructure.Storage
{
    public class GremlinServerFactory
    {
        public static GremlinServer Create()
        {
            return new GremlinServer(
                hostname:Credentials.Friends.Hostname
                , port: Credentials.Friends.Port
                , enableSsl:true
                , username:Credentials.Friends.Database
                , password:Credentials.Friends.AuthKey);
        }
    }
}
