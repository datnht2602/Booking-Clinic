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
    public class InvoiceRepository :  BaseRepository<Invoice>, IInvoiceRepository
    {
        private readonly IOptions<DatabaseSettingsOptions> databaseSettings;
        public InvoiceRepository(CosmosClient cosmosClient, IOptions<DatabaseSettingsOptions> databaseSettingsOption)
            : base(cosmosClient, databaseSettingsOption?.Value.DataBaseName, "Invoices")
        {
            this.databaseSettings = databaseSettingsOption;
        }
    }
}