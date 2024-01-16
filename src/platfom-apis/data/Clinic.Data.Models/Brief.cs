using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Models
{
    public class Brief
    {
        public string Address1 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string HealthInsuranceCode { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}