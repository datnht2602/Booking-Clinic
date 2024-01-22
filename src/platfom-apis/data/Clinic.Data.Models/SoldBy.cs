using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Models
{
    public class SoldBy
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string ManagerName { get; set; }


        public string Email { get; set; }


        public string Phone { get; set; }
    }
}