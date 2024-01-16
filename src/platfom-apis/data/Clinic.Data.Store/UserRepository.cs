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
    public class UserRepository : BaseRepository<Data.Models.User>, IUserRepository
    {
        private readonly IOptions<DatabaseSettingsOptions> databaseSettings;
        public UserRepository(CosmosClient cosmosClient, IOptions<DatabaseSettingsOptions> databaseSettingsOption)
            : base(cosmosClient, databaseSettingsOption?.Value.DataBaseName, "Users")
        {
            this.databaseSettings = databaseSettingsOption;
        }
    }
}