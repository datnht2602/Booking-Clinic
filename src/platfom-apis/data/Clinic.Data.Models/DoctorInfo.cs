using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Models
{
    public class DoctorInfo
    {
        public int ExperienceYear { get; set; }
        public string Title { get; set; }
        public int Specialization { get; set; }
        public string ClinicNum { get; set; }
    }
}