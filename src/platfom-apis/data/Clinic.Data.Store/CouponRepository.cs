using Clinic.Data.Models;
using Clinic.Data.Store.Contracts;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Clinic.Data.Store
{
    public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
    {
        private readonly IOptions<DatabaseSettingsOptions> databaseSettings;
        public CouponRepository(CosmosClient cosmosClient, IOptions<DatabaseSettingsOptions> databaseSettingsOption)
            : base(cosmosClient, databaseSettingsOption?.Value.DataBaseName, "Coupons")
        {
            this.databaseSettings = databaseSettingsOption;
        }
    }
}