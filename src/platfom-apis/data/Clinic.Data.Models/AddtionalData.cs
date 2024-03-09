using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Models
{
    public class AddtionalData
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string HealthInsuranceCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ClinicNumber { get; set; }
        public string PhoneNumber { get; set; }
    }
}