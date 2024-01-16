using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using Newtonsoft.Json;
namespace Clinic.Data.Models
{
    public class User
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }


        public string Name { get; set; }


        public string Email { get; set; }
        public Roles Role { get; set; }


        public int PhoneNumber { get; set; }


        public List<Brief> Brief { get; set; }
        public Detail Detail {get; set;}


        [JsonProperty("_etag")]
        public string Etag { get; set; }
    }

}