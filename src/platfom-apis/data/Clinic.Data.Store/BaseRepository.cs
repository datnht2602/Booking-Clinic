using Clinic.Data.Store.Contracts;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Store
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly Container container;
        public BaseRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            this.container = cosmosClient?.GetContainer(databaseName, containerName);
        }
        public async Task<ItemResponse<TEntity>> AddAsync(TEntity entity, string partitionKey) =>
            await this.container.CreateItemAsync<TEntity>(entity, new PartitionKey(partitionKey)).ConfigureAwait(false);
        
        public async Task<IEnumerable<TEntity>> GetAsync(string filterCriteria)
        {
            if (string.IsNullOrWhiteSpace(filterCriteria))
            {
                filterCriteria = "select * from e";
            }
            else
            {
                filterCriteria = $"select * from e where {filterCriteria}";
            }

            var iterator = this.container.GetItemQueryIterator<TEntity>(new QueryDefinition(filterCriteria));
            List<TEntity> results = new List<TEntity>();
            while (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync().ConfigureAwait(false);

                results.AddRange(result.ToList());
            }

            return results;
        }
        public async Task<TEntity> GetByNameCouponAsync(string filterCriteria)
        {
            if (string.IsNullOrWhiteSpace(filterCriteria))
            {
                filterCriteria = "select * from e";
            }
            else
            {
                filterCriteria = $"select * from e where e.CouponCode = \"{filterCriteria}\"";
            }

            var iterator = this.container.GetItemQueryIterator<TEntity>(new QueryDefinition(filterCriteria));
            List<TEntity> results = new List<TEntity>();
            while (iterator.HasMoreResults)
            {
                var result = await iterator.ReadNextAsync().ConfigureAwait(false);

                results.AddRange(result.ToList());
            }

            return results.FirstOrDefault();
        }
        public async Task<TEntity> GetByIdAsync(string id, string partitionKey)
        {
            try
            {
                ItemResponse<TEntity> response = await this.container.ReadItemAsync<TEntity>(id, new PartitionKey(partitionKey)).ConfigureAwait(false);
                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }
        public async Task<bool> ModifyAsync(TEntity entity, string etag, string partitionKey)
        {
            try
            {
                ItemResponse<TEntity> response = await this.container.UpsertItemAsync<TEntity>(entity, new PartitionKey(partitionKey), new ItemRequestOptions { IfMatchEtag = etag }).ConfigureAwait(false);
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound || ex.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
            {
                return false;
            }
        }
        public async Task<bool> RemoveAsync(string id, string partitionKey)
        {
            try
            {
                ItemResponse<TEntity> response = await this.container.DeleteItemAsync<TEntity>(id, new PartitionKey(partitionKey)).ConfigureAwait(false);
                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }
    }
}