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


        public int Specialization { get; set; }


        public int Price { get; set; }


        public DateTime CreatedDate { get; set; }


        public List<string> ImageUrls { get; set; }
        
        public double Rating { get; set; }
        
        public string Authors { get; set; }
        
        public string Description { get; set; }
        
        public bool IsMainCombo { get; set; }

        public List<Product> Products { get; set; }


        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }
}