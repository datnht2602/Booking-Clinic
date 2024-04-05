using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clinic.Data.Store
{
    public class BlogRepository : BaseRepository<Blog>, IBlogRepository
    {
        private readonly IOptions<DatabaseSettingsOptions> databaseSettings;

        public BlogRepository(CosmosClient cosmosClient, IOptions<DatabaseSettingsOptions> databaseSettingsOption)
            : base(cosmosClient, databaseSettingsOption?.Value.DataBaseName, "Blogs")
        {
            this.databaseSettings = databaseSettingsOption;
        }
    }
}
