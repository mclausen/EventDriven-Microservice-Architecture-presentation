using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace SharedKernel
{
    public sealed class DocumentDbRepository<T> where T : class
    {
        private static readonly string DatabaseId = Credentials.DocumentDb.DatabaseId;
        private static DocumentClient client;

        public static async Task<T> GetItemAsync(string id, string partitionKey, string collectionId)
        {
            try
            {
                Document document =
                    await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, collectionId, id), new RequestOptions()
                    {
                        PartitionKey = new PartitionKey(partitionKey)
                    });
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, string collectionId)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, collectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true})
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<Document> CreateItemAsync(T item, string partitionKey, string collectionId)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, collectionId), item, new RequestOptions()
            {
                PartitionKey = new PartitionKey(partitionKey)
            });
        }

        public static async Task<Document> UpdateItemAsync(string id, string collectionId, string partitionKey, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, collectionId, id), item, new RequestOptions()
            {
                PartitionKey = new PartitionKey(partitionKey)
            });
        }

        public static async Task DeleteItemAsync(string id, string collectionId)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, collectionId, id));
        }

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(Credentials.DocumentDb.EndpointUrl), Credentials.DocumentDb.AuthorizationKey, new ConnectionPolicy()
            {
                ConnectionMode = ConnectionMode.Direct,
                RetryOptions = new RetryOptions()
                {
                    MaxRetryAttemptsOnThrottledRequests = 5,
                    MaxRetryWaitTimeInSeconds = 5 
                },
                ConnectionProtocol = Protocol.Tcp,
            });
        }

        public static void Teardown()
        {
            DeleteDatabaseAsync().Wait();
        }

        private static async Task DeleteDatabaseAsync()
        {
            await client.DeleteDatabaseAsync((UriFactory.CreateDatabaseUri(DatabaseId)));
        }
    }
}
