using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Clinic.Data.Models
{
    public class Invoice
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }


        public string OrderId { get; set; }

        public double OrderTotal { get; set; }
        public int PaymentMode { get; set; }

        public string Coupon { get; set; }
        public AddtionalData AdditionalData { get; set; }


        public SoldBy SoldBy { get; set; }


        public List<Product> Product { get; set; }


        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}