using Newtonsoft.Json;

namespace Clinic.Data.Models
{
    public class Coupon
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CouponCode { get; set; }
        public int Quantity { get; set; }
        public double DiscountAmount { get; set; }
        public bool IsEnable { get; set; }
        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}