using System;

namespace SharedKernel
{
    public class Credentials
    {
        public class Friends
        {
            public static string Hostname = "";
            public static int Port = 443;
            public static string AuthKey = "";
            public static string Database = "/dbs/Friends/colls/friends";
        }

        public class DocumentDb
        {
            public static readonly string DatabaseId = "event-driven-microservice-architecture";
            public static readonly string EndpointUrl = "";
            public static readonly string AuthorizationKey = "";
        }

        public class ServiceBus
        {
            public static string ConnectionString = "";
        }
        
    }
}
