using Newtonsoft.Json;
using System;

namespace Clinic.Data.Models
{
    public class Blog
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageUrl { get; set; }

        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}
