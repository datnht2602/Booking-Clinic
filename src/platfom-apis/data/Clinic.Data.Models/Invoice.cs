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
        public string PaymentMode { get; set; }
        public Brief Brief { get; set; }
        public SoldBy SoldBy { get; set; }
        public List<Service> Services { get; set; }
        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}