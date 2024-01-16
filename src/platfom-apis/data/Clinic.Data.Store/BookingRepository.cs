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
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        private readonly IOptions<DatabaseSettingsOptions> databaseSettings;
        public BookingRepository(CosmosClient cosmosClient, IOptions<DatabaseSettingsOptions> databaseSettingsOption)
            : base(cosmosClient, databaseSettingsOption?.Value.DataBaseName, "Booking")
        {
            this.databaseSettings = databaseSettingsOption;
        }
    }
}