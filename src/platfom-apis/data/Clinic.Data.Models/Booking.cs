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
        public string UserName { get; set; }
        public int Specialization { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Description { get; set; }
        public string CouponCode { get; set; }
        public double DiscountTotal { get; set; }
        public int CartTotalItems { get; set; }
        public List<Product> Products { get; set; }
        public string OrderStatus { get; set; } 
        public long OrderPlacedDate { get; set; }
        public AddtionalData AdditionalData { get; set; }
        public double OrderTotal { get; set; }
        public int PaymentMode { get; set; }

        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}