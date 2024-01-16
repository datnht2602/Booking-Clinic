using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Clinic.Data.Store
{
    public class ServiceRepository : BaseRepository<Service> , IServiceRepository
    {
        private readonly IOptions<DatabaseSettingsOptions> databaseSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRepository"/> class.
        /// </summary>
        /// <param name="cosmosClient">The cosmos client.</param>
        /// <param name="databaseSettingsOption">The database settings option.</param>
        public ServiceRepository(CosmosClient cosmosClient, IOptions<DatabaseSettingsOptions> databaseSettingsOption)
            : base(cosmosClient, databaseSettingsOption?.Value.DataBaseName, "Services")
        {
            this.databaseSettings = databaseSettingsOption;
        }
    }
}