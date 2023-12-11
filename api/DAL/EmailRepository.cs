using System;
using System.Linq;
using System.Threading.Tasks;
using Company.Function.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Company.Function.DAL
{
    public class EmailRepository
    {
        private static readonly string ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        private const string DatabaseId = "email-subscriptions";
        private const string ContainerId = "emails";
        private const string PartitionKey = "/id";
        
        // The Cosmos client instance
        private readonly CosmosClient _cosmosClient = new(ConnectionString);

        // The database we will create
        private Database _database;

        // The container we will create.
        private Container _container;

        public async Task<bool> Contains(string email)
        {
            var queryable = this._container.GetItemLinqQueryable<EmailModel>();
            var matches = queryable.Where(e => e.Email == email);
            using FeedIterator<EmailModel> linqFeed = matches.ToFeedIterator();
            while (linqFeed.HasMoreResults)
            {
                var response = await linqFeed.ReadNextAsync();
                return response.Count > 0;
            }
            return false;
        }

        public async Task<EmailModel> Add(EmailModel email)
        {
            await Initialize();

            if (await Contains(email.Email))
            {
                return null;
            }

            email.Id = Guid.NewGuid().ToString();
            return await _container.CreateItemAsync<EmailModel>(email);
        }
        public async Task<EmailModel> Get(string id)
        {
            await Initialize();
            
            return await _container.ReadItemAsync<EmailModel>(id, new PartitionKey(id));
        }
        public async Task<EmailModel> Delete(EmailModel item)
        {
            await Initialize();
            
            return await _container.DeleteItemAsync<EmailModel>(item.Id ,new PartitionKey(item.Id));
        }

        private async Task Initialize()
        {
            _database ??= await _cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            _container ??= await _database.CreateContainerIfNotExistsAsync(ContainerId, PartitionKey);
        }
    }
}
