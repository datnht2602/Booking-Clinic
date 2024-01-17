using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Clinic.Data.Models
{
    public class Booking
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string UserId { get; set; }

        public List<Product> Products { get; set; }


        public string OrderStatus { get; set; }

  
        public string OrderPlacedDate { get; set; }


        public Brief Brief { get; set; }

        public double OrderTotal { get; set; }


        public string PaymentMode { get; set; }


        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}