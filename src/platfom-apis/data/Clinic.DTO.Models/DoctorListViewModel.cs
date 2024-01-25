using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public record DoctorListViewModel(string Id, string Name)
    {
 
        public List<Uri> ImageUrls { get; set; }
        public int ExperienceYear { get; set; }
        public string Title { get; set; }
        public Specialization Specialization { get; set; }
        public string ClinicNum { get; set; }
        public double AverageRating { get; set; }
    }

    public class Specialization
    {
    }
}