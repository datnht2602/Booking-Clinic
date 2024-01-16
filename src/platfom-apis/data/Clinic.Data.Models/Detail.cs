using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Data.Models
{
    public class Detail
    {
        public int ExperienceYear { get; set; }
        public string Title { get; set; }
        public Specialization Specialization { get; set; }
        public List<Rating> Rating { get; set; }
        public string ClinicNum { get; set; }
    }

    public enum Specialization
    {
    }
}