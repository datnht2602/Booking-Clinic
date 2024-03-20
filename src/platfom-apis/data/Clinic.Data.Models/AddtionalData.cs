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
        public string OrderPlacedTime { get; set; }
        public int Sexual { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int ClinicNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}