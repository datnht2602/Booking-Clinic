using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Clinic.Data.Models
{
    public class Product
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }


        public string Name { get; set; }


        public string Specialization { get; set; }


        public int Price { get; set; }


        public DateTime CreatedDate { get; set; }


        public List<string> ImageUrls { get; set; }


        public List<Rating> Rating { get; set; }


        public List<int> Users { get; set; }


        public List<string> ProductIds { get; set; }


        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}