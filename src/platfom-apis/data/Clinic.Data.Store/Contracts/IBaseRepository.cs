using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
namespace Clinic.Data.Store.Contracts
{
    using System.Collections.Generic;


    /// <summary>
    /// The generic repository.
    /// </summary>
    /// <typeparam name="TEntity">Generic entity if type class.</typeparam>
    public interface IBaseRepository<TEntity>
    where TEntity : class
    {
        /// <summary>
        /// Get records from a container based on filterCriteria.
        /// </summary>
        /// <param name="filterCriteria">Optional filter criteria.</param>
        /// <returns>The task.</returns>
        Task<IEnumerable<TEntity>> GetAsync(string filterCriteria);

        /// <summary>
        /// This method helps in retrieving any record from a container by its id and partition key.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <param name="partitionKey">Partition key.</param>
        /// <returns>The entity.</returns>
        Task<TEntity> GetByIdAsync(string id, string partitionKey);

        /// <summary>
        /// Allows to insert a record in a container.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="partitionKey">The partition key.</param>
        /// <returns>The ItemResponse.</returns>
        Task<ItemResponse<TEntity>> AddAsync(TEntity entity, string partitionKey);

        /// <summary>
        /// This method allows to upsert (modify if record is present else insert) a record in  a container.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="partitionKey">the partition key.</param>
        /// <returns>success or failure of modification.</returns>
        Task<bool> ModifyAsync(TEntity entity, string etag, string partitionKey);

        /// <summary>
        /// This method allows deletion of a record from a container.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <param name="partitionKey">Partition key.</param>
        /// <returns>success or failure of deletion.</returns>
        Task<bool> RemoveAsync(string id, string partitionKey);
    }
}