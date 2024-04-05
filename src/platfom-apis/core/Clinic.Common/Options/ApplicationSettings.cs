using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Common.Options
{
    public class ApplicationSettings
    {
        public bool IncludeExceptionStackInResponse { get; set; }
        public bool UseRedisCache { get; set; }
        public string DataStoreEndpoint { get; set; }
        public string ProductsApiEndpoint { get; set; }
        public string InvoiceApiEndpoint { get; set; }
        public string OrdersApiEndpoint { get; set; }
        public string IdentityApiEndpoint { get; set; }
        public string CouponApiEndpoint { get; set; }
        public string ApiGatewayEndpoint { get; set; }
        public string EmailEndpoint { get; set; }
        public string ClientApiEndpoint { get; set; }
        public string BlogApiEndpoint { get; set; }       
    }
}
